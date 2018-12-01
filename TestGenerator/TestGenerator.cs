using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace TestGenerator
{
    public class TestGenerator
    {
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
            
        }
    }
}