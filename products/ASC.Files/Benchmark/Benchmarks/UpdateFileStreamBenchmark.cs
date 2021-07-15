using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

using ASC.Files.Benchmark.TestsConfiguration;
using ASC.Files.Benchmark.Utils;

using BenchmarkDotNet.Attributes;

namespace ASC.Files.Benchmark.Benchmarks
{
    [Config(typeof(UpdateFileStreamTestConfig))]
    public class UpdateFileStreamBenchmark : BenchmarkBase
    {
        private int _fileId;
        private List<int> _filesId = new List<int>();
        private Task[] _tasks;
        private List<TestStream> _testStreams = new List<TestStream>();

        public IEnumerable<TestStream> TestStreams()
        {
            yield return StreamGenerator.Generate(1024);
            yield return StreamGenerator.Generate(2048);
            yield return StreamGenerator.Generate(4096);
        }

        #region UpdateFileStreamTest
        [GlobalSetup(Target = nameof(UpdateFileStreamTest))]
        public void IterSetupUpdateFileStreamTest()
        {
            _fileId = _dataStorage.Users[0].CreateFileInMy();
        }

        [Benchmark]
        [ArgumentsSource(nameof(TestStreams))]
        public void UpdateFileStreamTest(TestStream testStream)
        {
            testStream.Stream.Position = 0;
            _dataStorage.Users[0].UpdateFileStream(_fileId, testStream.Stream);
        }
        #endregion

        #region UpdateFileStreamManyUsersTest
        [GlobalSetup(Target = nameof(UpdateFileStreamManyUsersTest))]
        public void GlobalSetupUpdateFileStreamManyUsersTest()
        {
            foreach (var user in _dataStorage.Users)
            {
                _filesId.Add(user.CreateFileInMy());
            }

            var testStream = StreamGenerator.Generate(10240);

            for (int i = 0; i < _dataStorage.Users.Count; i++)
            {
                testStream.Stream.Position = 0;
                var memoryStream = new MemoryStream();
                testStream.Stream.CopyTo(new MemoryStream());

                _testStreams.Add(new TestStream(memoryStream));
            }
        }

        [IterationSetup(Target = nameof(UpdateFileStreamManyUsersTest))]
        public void IterSetupUpdateFileStreamManyUsersTest()
        {
            _tasks = new Task[_dataStorage.Users.Count];

            for (int i = 0; i < _dataStorage.Users.Count; i++)
            {
                var user = _dataStorage.Users[i];
                var fileId = _filesId[i];
                var testStream = _testStreams[i];

                testStream.Stream.Position = 0;

                _tasks[i] = new Task(() =>
                {
                    user.UpdateFileStream(fileId, testStream.Stream);
                });
            }
        }

        [Benchmark]
        public void UpdateFileStreamManyUsersTest()
        {
            foreach (var task in _tasks)
            {
                task.Start();
            }

            Task.WaitAll(_tasks);
        }
        #endregion
    }
}
