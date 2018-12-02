using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using TestGenerator;

namespace TestGeneratorClass
{
    public class TestGeneratorClass
    {
        private Produce produceClass;
        private Reader reader;
        private Writer writer;

        public TestGeneratorClass(string pathOutputDir)
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
                new ActionBlock<TestClassSignature>(genClass => writer.write(genClass), outputTaskOptions);

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