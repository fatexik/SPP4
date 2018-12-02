using System.Collections.Generic;
using System.Linq;

namespace TestGenerator
{
    public class Produce
    {
        public TestClassSignature produce(string code)
        {
            TreeBuilder treeBuilder = new TreeBuilder(code);
            SyntaxTreeInfo syntaxTreeInfo = treeBuilder.synteseTreeInfo();
            
            TestClassGenerator testClassGenerator = new TestClassGenerator(syntaxTreeInfo);
            List<TestClassSignature> testTemplates = testClassGenerator.GetTestTemplates();
            
            return new TestClassSignature(testTemplates.First().getTestClassName(),testTemplates.First().getTestClassData());
        }
    }
}