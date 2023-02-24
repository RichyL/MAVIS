using MAVIS_Source_Generators;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Diagnostics;

namespace SourceGeneratorTests
{
    [TestClass]
    public class MAVIS_MethodTests
    {
        [TestMethod]
        public void AddMAVISMethodsToPartialClass()
        {

            var mavisMethodSourceGenerator = new MAVISMethodGenerator();
            var codeReadFromFile = File.ReadAllText("C:\\Users\\richy\\source\\repos\\RichyL\\MAVIS\\SourceGeneratorTests\\SourceGeneratorTests\\FirstViewModel.txt");
            var programCode = CSharpSyntaxTree.ParseText(codeReadFromFile);
            var result = TestHelper.GetGeneratorDriverRunResult( new[] { programCode }, new[] { mavisMethodSourceGenerator });
            Debug.WriteLine(result.GeneratedTrees.Count());
            Assert.IsTrue(result.GeneratedTrees.Any());
        }
    }
}