using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace SourceGeneratorTests
{
    public class TestHelper
    {
        public static GeneratorDriverRunResult GetGeneratorDriverRunResult(IEnumerable<SyntaxTree> sourceCode, IIncrementalGenerator[] generators)
        {
            var compilationOptions = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary);
            var inputCompilation = CSharpCompilation.Create("compilationAssemblyName", sourceCode, null, compilationOptions);
            var driver = (GeneratorDriver)CSharpGeneratorDriver.Create(generators);
            driver = driver.RunGeneratorsAndUpdateCompilation(inputCompilation, out _, out _);
            return driver.GetRunResult();
        }
    }

}