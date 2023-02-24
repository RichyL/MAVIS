using FluentAssertions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Xunit;

namespace SourceGeneratorTests
{
    public class UnitTest1
    {
        private static string? GetGeneratedOutput(string sourceCode)
        {
            var syntaxTree = CSharpSyntaxTree.ParseText(sourceCode);
            var references = AppDomain.CurrentDomain.GetAssemblies()
                                      .Where(assembly => !assembly.IsDynamic)
                                      .Select(assembly => MetadataReference
                                                          .CreateFromFile(assembly.Location))
                                      .Cast<MetadataReference>();

            var compilation = CSharpCompilation.Create("SourceGeneratorTests",
                          new[] { syntaxTree },
                          references,
                          new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

            // Source Generator to test
            var generator = new MAVIS_Source_Generators.MAVISSourceGenerator();

            CSharpGeneratorDriver.Create(generator)
                                 .RunGeneratorsAndUpdateCompilation(compilation,
                                                                    out var outputCompilation,
                                                                    out var diagnostics);

            // optional
            diagnostics.Where(d => d.Severity == DiagnosticSeverity.Error)
                       .Should().BeEmpty();

            return outputCompilation.SyntaxTrees.Skip(1).LastOrDefault()?.ToString();
        }

        [Fact]
        public void Test1()
        {
            // Create the 'input' compilation that the generator will act on
            StreamReader sr = new StreamReader("C:\\Users\\richy\\source\\repos\\RichyL\\MAVIS\\Richy_WPF_MVVM\\User\\ViewModels\\FirstViewModel.cs");
            string content = sr.ReadToEnd();

            GetGeneratedOutput(content).Should().BeNull();
        }
    }
}