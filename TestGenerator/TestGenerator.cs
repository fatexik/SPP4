using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace TestGenerator
{
    public class TestGenerator
    {
        private Produce produceClass;
        private Reader reader;
        private Writer writer;

        public TestGenerator(string pathOutputDir)
        {
            produceClass = new Produce();
            reader = new Reader();
            writer = new Writer(pathOutputDir);
        }

        public async Task generate(int degreeParall, int outputParall, List<string> sourcePath)
        {
            DataflowLinkOptions linkOptions = new DataflowLinkOptions();
            linkOptions.PropagateCompletion = true;
            ExecutionDataflowBlockOptions processingTaskOptions = new ExecutionDataflowBlockOptions();
            ExecutionDataflowBlockOptions outputTaskOptions = new ExecutionDataflowBlockOptions();
            processingTaskOptions.MaxDegreeOfParallelism = degreeParall;
            outputTaskOptions.MaxDegreeOfParallelism = outputParall;

            TransformBlock<string, string> readingBlock =
                new TransformBlock<string, string>(s => reader.ReadAsync(s), processingTaskOptions);

            TransformBlock<string, TestClassSignature> producingBlock =
                new TransformBlock<string, TestClassSignature>(s => produceClass.produce(s), processingTaskOptions);

            ActionBlock<TestClassSignature> writingBlock =
                new ActionBlock<TestClassSignature>(genClass => writer.write(genClass).Wait(),outputTaskOptions);

            readingBlock.LinkTo(producingBlock, linkOptions);
            producingBlock.LinkTo(writingBlock, linkOptions);

            foreach (var path in sourcePath)
            {
                readingBlock.Post(path);
            }
            
            readingBlock.Complete();
            await writingBlock.Completion;
        }
    }
}