using System.ComponentModel;
using Avaruz.Framework.SourceGenerators;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace NotifyPropertyChangedGenerator.Tests
{
  public class GeneratorTests
  {
    [Test]
    public void TestBasicPropertyGeneration()
    {
      var testCode = @"
                using System.ComponentModel;
                namespace TestNamespace
                {
                    [NotifyPropertyChanged]
                    public partial class TestClass
                    {
                        public string Name { get; set; }
                        public string LastName { get; set; }
                    }
                }";

      var compilation = CSharpCompilation.Create("TestCompilation",
          syntaxTrees: new[] { CSharpSyntaxTree.ParseText(testCode) },
          references: new[] { MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
                                    MetadataReference.CreateFromFile(typeof(INotifyPropertyChanged).Assembly.Location) });

      var generator = new AutoNotifyPropertyChangedGenerator();
      var driver = CSharpGeneratorDriver.Create(generator);

      driver = (CSharpGeneratorDriver)driver.RunGeneratorsAndUpdateCompilation(compilation, out var outputCompilation, out var diagnostics);

      // Verify the outputCompilation for the generated code

      Assert.That(2, Is.EqualTo(outputCompilation.SyntaxTrees.Count()));


      var generatedSyntaxTree = outputCompilation.SyntaxTrees.Last();
      var generatedCode = generatedSyntaxTree.ToString();

      // Add assertions to verify the generated code
      Assert.That(generatedCode, Does.Contain("public partial class TestClass : INotifyPropertyChanged"));
      Assert.That(generatedCode, Does.Contain("private string _name;"));
      Assert.That(generatedCode, Does.Contain("public string Name"));
      Assert.That(generatedCode, Does.Contain("OnPropertyChanged(nameof(Name));"));
    }
  }
}

