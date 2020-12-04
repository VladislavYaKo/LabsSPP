using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using NUnitTestGenerator;

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
                NUnitTestGenerator.NUnitTestGenerator nUnitTestGenerator = new NUnitTestGenerator.NUnitTestGenerator();
                var generatedTests = nUnitTestGenerator.Generate(text);
                List<GeneratedTestsFile> results = new List<GeneratedTestsFile>();
                if (generatedTests == null)
                {
                    Console.WriteLine("Test methods cannot be generated.");
                    return results;
                }

                foreach (var generatedTest in generatedTests)
                {
                    string generatedTestStr = generatedTest.ToString();
                    string testName = "";
                    string[] words = generatedTestStr.Split(' ');
                    for (int i = 0; i < words.Length; i++)
                    {
                        if (words[i] == "class")
                        {
                            i++;
                            testName = words[i].Trim(' ', '\r', '\n', '\t');
                        }
                    }
                    results.Add(new GeneratedTestsFile(Path.GetFullPath(_testFolder) + "\\" + testName + "Test.cs", generatedTestStr));
                }
                return results;
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
