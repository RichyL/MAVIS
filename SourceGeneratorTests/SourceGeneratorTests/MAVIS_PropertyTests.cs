using MAVIS_Source_Generators;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Diagnostics;

namespace SourceGeneratorTests
{
    [TestClass]
    public class MAVIS_PropertyTests
    {
        [TestMethod]
        public void AddMAVISPropertiesToPartialClass()
        {

            var mavisPropertySourceGenerator = new MAVISPropertyGenerator();
            var codeReadFromFile = File.ReadAllText("C:\\Users\\richy\\source\\repos\\RichyL\\MAVIS\\SourceGeneratorTests\\SourceGeneratorTests\\FirstViewModel.txt");
            var programCode = CSharpSyntaxTree.ParseText(codeReadFromFile);
            var result = TestHelper.GetGeneratorDriverRunResult( new[] { programCode }, new[] { mavisPropertySourceGenerator });
            Debug.WriteLine(result.GeneratedTrees.Count());
            Assert.IsTrue(result.GeneratedTrees.Any());
        }
    }
}