using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

using ASC.Files.Benchmark.BenchmarkEnviroment;
using ASC.Files.Benchmark.TestsConfiguration;
using ASC.Files.Benchmark.Utils;

using BenchmarkDotNet.Attributes;

namespace ASC.Files.Benchmark.Benchmarks
{
    [Config(typeof(UpdateFileStreamTestConfig))]
    public class UpdateFileStreamBenchmark
    {
        private TestDataStorage _usersStorage = TestDataStorage.GetStorage();
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

        [GlobalSetup(Target = nameof(UpdateFileStreamTest))]
        public void IterSetupUpdateFileStreamTest()
        {
            _fileId = _usersStorage.Users[0].CreateFileInMy();
        }

        [Benchmark]
        [ArgumentsSource(nameof(TestStreams))]
        public void UpdateFileStreamTest(TestStream testStream)
        {
            testStream.Stream.Position = 0;
            _usersStorage.Users[0].UpdateFileStream(_fileId, testStream.Stream);
        }

        [GlobalSetup(Target = nameof(UpdateFileStreamUsersTest))]
        public void GlobalSetupUpdateFileStreamUsersTest()
        {
            foreach (var user in _usersStorage.Users)
            {
                _filesId.Add(user.CreateFileInMy());
            }

            var testStream = StreamGenerator.Generate(10240);

            for (int i = 0; i < _usersStorage.Users.Count; i++)
            {
                testStream.Stream.Position = 0;
                var memoryStream = new MemoryStream();
                testStream.Stream.CopyTo(new MemoryStream());

                _testStreams.Add(new TestStream(memoryStream));
            }
        }

        [IterationSetup(Target = nameof(UpdateFileStreamUsersTest))]
        public void IterSetupUpdateFileStreamUsersTest()
        {
            _tasks = new Task[_usersStorage.Users.Count];

            for (int i = 0; i < _usersStorage.Users.Count; i++)
            {
                var user = _usersStorage.Users[i];
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
        public void UpdateFileStreamUsersTest()
        {
            foreach (var task in _tasks)
            {
                task.Start();
            }

            Task.WaitAll(_tasks);
        }
    }
}
