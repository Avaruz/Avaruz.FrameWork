using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using Microsoft.CodeAnalysis.Editing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;

namespace Avaruz.Framework.SourceGenerators
{
  [Generator]
  public class NotifyPropertyChangedGenerator : ISourceGenerator
  {
    public void Initialize(GeneratorInitializationContext context)
    {
      // No es necesario realizar ninguna inicialización especial.
    }

    public void Execute(GeneratorExecutionContext context)
    {
      // Recopila la información de las clases y propiedades en el proyecto.
      var compilation = context.Compilation;

      // Obtén todas las clases del proyecto.
      var allClasses = compilation.SyntaxTrees
          .SelectMany(tree => tree.GetRoot().DescendantNodes().OfType<ClassDeclarationSyntax>());

      // Filtra las clases que tienen la etiqueta [NotifyPropertyChanged].
      var candidateClasses = allClasses.Where(classSyntax =>
          classSyntax.AttributeLists.Any(attrList =>
              attrList.Attributes.Any(attr =>
                  attr.Name.ToString() == "NotifyPropertyChanged")));

      // Para cada clase candidata.
      foreach (var classDeclaration in candidateClasses)
      {
        var classSymbol = context.Compilation.GetSemanticModel(classDeclaration.SyntaxTree).GetDeclaredSymbol(classDeclaration);
        if (classSymbol != null)
        {
          var properties = classSymbol.GetMembers().OfType<IPropertySymbol>();
          var className = classSymbol.Name;

          // Genera el código de notificación de cambios de propiedad para cada propiedad.
          var propertyChangeCode = properties.Select(property =>
          {
            var propertyName = property.Name;
            var fieldName = LowercaseFirstLetter(property.Name);
            return SyntaxFactory.ParseSyntaxTree($@"
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
                    ").GetRoot();
          });

          // Combina el código de notificación de cambios de propiedad en una clase parcial.
          var partialClass = SyntaxFactory.ParseSyntaxTree($@"
                    using System.ComponentModel;
                    public partial class {className}:INotifyPropertyChanged
                    {{
                        {string.Join("\n", propertyChangeCode)}
                        
                        public event PropertyChangedEventHandler PropertyChanged;
                        private void OnPropertyChanged(string propertyName)
                        {{
                            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
                        }}
                    }}
                ").GetRoot();

          // Agrega la clase parcial generada al árbol de sintaxis del proyecto.
          context.AddSource($"{className}_NotifyPropertyChangedPartial.cs", partialClass.NormalizeWhitespace().ToFullString());
        }
      }

      string LowercaseFirstLetter(string word)
      {
        return word[0].ToString().ToLower() + word.Substring(1);
      }
    }
  }
}