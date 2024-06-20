using System.Collections.Immutable;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace Avaruz.Framework.SourceGenerators
{
  [Generator]
  public class AutoNotifyPropertyChangedGenerator : IIncrementalGenerator
  {
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
      var classDeclarations = context.SyntaxProvider
          .CreateSyntaxProvider(
              predicate: static (s, _) => IsClassWithAttribute(s),
              transform: static (ctx, _) => GetSemanticTargetForGeneration(ctx))
          .Where(static m => m is not null);

      var compilationAndClasses = context.CompilationProvider.Combine(classDeclarations.Collect());

      context.RegisterSourceOutput(compilationAndClasses,
          static (spc, source) => Execute(source.Left, source.Right, spc));
    }

    static bool IsClassWithAttribute(SyntaxNode node)
    {
      return node is ClassDeclarationSyntax classDeclaration &&
             classDeclaration.AttributeLists
                 .Any(attrList => attrList.Attributes
                     .Any(attr => attr.Name.ToString() == "NotifyPropertyChanged"));
    }

    static ClassDeclarationSyntax? GetSemanticTargetForGeneration(GeneratorSyntaxContext context)
    {
      var classDeclaration = (ClassDeclarationSyntax)context.Node;

      foreach (var attributeList in classDeclaration.AttributeLists)
      {
        foreach (var attribute in attributeList.Attributes)
        {
          var name = attribute.Name.ToString();
          if (name == "NotifyPropertyChanged")
          {
            return classDeclaration;
          }
        }
      }

      return null;
    }

    static void Execute(Compilation compilation, ImmutableArray<ClassDeclarationSyntax> classes, SourceProductionContext context)
    {
      if (classes.IsDefaultOrEmpty)
      {
        return;
      }

      foreach (var classDeclaration in classes)
      {
        var model = compilation.GetSemanticModel(classDeclaration.SyntaxTree);
        var classSymbol = model.GetDeclaredSymbol(classDeclaration);

        if (classSymbol == null)
        {
          continue;
        }

        var properties = (classSymbol as INamedTypeSymbol)?.GetMembers().OfType<IPropertySymbol>()
                                       .Where(prop => !prop.IsStatic && prop.SetMethod != null);

        var className = classSymbol.Name;
        var namespaceName = classSymbol.ContainingNamespace.ToDisplayString();

        var propertyChangeCode = new StringBuilder();
        foreach (var property in properties)
        {
          var propertyName = property.Name;
          var fieldName = LowercaseFirstLetter(propertyName);

          propertyChangeCode.Append($@"
                        private {property.Type} _{fieldName};
                        public {property.Type} {propertyName}
                        {{
                            get => _{fieldName};
                            set
                            {{
                                if (!Equals(_{fieldName}, value))
                                {{
                                    _{fieldName} = value;
                                    OnPropertyChanged(nameof({propertyName}));
                                }}
                            }}
                        }}
                    ");
        }

        var partialClass = $@"
                    using System.ComponentModel;

                    namespace {namespaceName}
                    {{
                        public partial class {className} : INotifyPropertyChanged
                        {{
                            {propertyChangeCode}

                            public event PropertyChangedEventHandler PropertyChanged;

                            private void OnPropertyChanged(string propertyName)
                            {{
                                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
                            }}
                        }}
                    }}
                ";

        context.AddSource($"{className}_NotifyPropertyChangedPartial.cs", SourceText.From(partialClass, Encoding.UTF8));
      }
    }

    static string LowercaseFirstLetter(string word)
    {
      return char.ToLower(word[0]) + word.Substring(1);
    }
  }
}
