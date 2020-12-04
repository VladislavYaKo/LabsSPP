using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace Lab4SPP
{
    public class Generator
    {
        private List<string> _filesForTest;
        private string _testFolder;
        private int _maxThreads;
        private int _maxFilesToRead;
        private int _maxFilesToWrite;

        public Generator(List<string> files, string folder, int maxThreads, int maxFilesToRead, int maxFilesToWrite)
        {
            _filesForTest = files;
            _testFolder = folder;
            _maxThreads = maxThreads;
            _maxFilesToRead = maxFilesToRead;
            _maxFilesToWrite = maxFilesToWrite;
        }

        public async Task GenerateAsync()
        {
            var readFile = new TransformBlock<string, string>(async path =>
            {
                string fileContent;
                using (StreamReader reader = File.OpenText(path))
                {
                    fileContent = await reader.ReadToEndAsync();
                }
                return fileContent;

            }, new ExecutionDataflowBlockOptions
            {
                MaxDegreeOfParallelism = _maxFilesToRead
            });

            var getTest = new TransformBlock<string, List<GeneratedTestsFile>>(text =>
            {
                //TODO
                return new List<GeneratedTestsFile>();
            }, new ExecutionDataflowBlockOptions
            {
                MaxDegreeOfParallelism = _maxThreads
            });

            var writeFile = new ActionBlock<List<GeneratedTestsFile>>(async testFiles =>
            {
                foreach (var testFile in testFiles)
                {
                    using (StreamWriter writer = File.CreateText(testFile.fileName))
                    {
                        await writer.WriteAsync(testFile.contentText);
                    }
                }
            }, new ExecutionDataflowBlockOptions
            {
                MaxDegreeOfParallelism = _maxFilesToWrite
            });

            var pipelineOptions = new DataflowLinkOptions { PropagateCompletion = true };

            readFile.LinkTo(getTest, pipelineOptions);
            getTest.LinkTo(writeFile, pipelineOptions);
            foreach(string file in _filesForTest)
            {
                readFile.Post(file);
            }

            readFile.Complete();

            await writeFile.Completion;
        }

    }
}
