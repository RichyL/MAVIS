using MAVIS_Source_Generators;
using Microsoft.CodeAnalysis;
using System.Diagnostics;

namespace SourceGeneratorTests
{
    [TestClass]
    public class MAVISAttributeGeneratorTests
    {
        [TestMethod]
        public void TestAttributesGenerated()
        {
            var attributeGenerator = new MAVISAttributeGenerator();
            var result = TestHelper.GetGeneratorDriverRunResult(Array.Empty<SyntaxTree>(), new[] { attributeGenerator });
            Debug.WriteLine(result.GeneratedTrees.Count());
            Assert.IsTrue(result.GeneratedTrees.Count()>0);
        }

        
    }
}
