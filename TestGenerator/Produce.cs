namespace TestGenerator
{
    public class Produce
    {
        public TestClassSignature produce(string code)
        {
            TreeBuilder treeBuilder = new TreeBuilder(code);
            SyntaxTreeInfo syntaxTreeInfo = treeBuilder.synteseTreeInfo();
            return null;
        }
    }
}