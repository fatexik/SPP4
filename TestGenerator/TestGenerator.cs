using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace TestGenerator
{
    public class TestGenerator
    {
        private Produce produceClass;
        public TestGenerator()
        {
            produceClass = new Produce();
        }
        
        public async Task generate(int degreeParall, int outputParall, Reader reader)
        {
            DataflowLinkOptions linkOptions = new DataflowLinkOptions();
            linkOptions.PropagateCompletion = true;
            ExecutionDataflowBlockOptions processingTaskOptions = new ExecutionDataflowBlockOptions();
            ExecutionDataflowBlockOptions outputTaskOptions = new ExecutionDataflowBlockOptions();
            processingTaskOptions.MaxDegreeOfParallelism = degreeParall;
            outputTaskOptions.MaxDegreeOfParallelism = outputParall;

            TransformBlock<string, string> readingBlock =
                new TransformBlock<string, string>(s => reader.ReadAsync(s), processingTaskOptions);
            
            TransformBlock<string,TestClassSignature> producingBlock = new TransformBlock<string, TestClassSignature>(s => produceClass.produce(s),outputTaskOptions);
            
        }
    }
}