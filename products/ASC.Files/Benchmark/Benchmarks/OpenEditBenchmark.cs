using ASC.Files.Benchmark.BenchmarkEnviroment;
using ASC.Files.Benchmark.TestsConfiguration;

using BenchmarkDotNet.Attributes;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace ASC.Files.Benchmark.Benchmarks
{
    [Config(typeof(OpenEditTestConfig))]
    public class OpenEditBenchmark
    {
        private TestDataStorage _usersStorage = TestDataStorage.GetStorage();
        private int _fileId;
        private List<int> _filesId = new List<int>();
        private Task[] _tasks;

        [GlobalSetup(Target =nameof(OpenEditTest))]
        public void GlobalSetupOpenEditTest()
        {
            _fileId = _usersStorage.Users[0].CreateFileInMy();
        }

        [Benchmark]
        public void OpenEditTest()
        {
            _usersStorage.Users[0].OpenEdit(_fileId);
        }

        [GlobalSetup(Target = nameof(OpenEditUsersTest))]
        public void GlobalSetupOpenEditUsersTest()
        {
            foreach (var user in _usersStorage.Users)
            {
                _filesId.Add(user.CreateFileInMy());
            }
        }

        [IterationSetup(Target = nameof(OpenEditUsersTest))]
        public void IterSetupOpenEditUsersTest()
        {
            _tasks = new Task[_usersStorage.Users.Count];

            for (int i = 0; i < _usersStorage.Users.Count; i++)
            {
                var user = _usersStorage.Users[i];
                var fileId = _filesId[i];

                _tasks[i] = new Task(() =>
                {
                    user.OpenEdit(fileId);
                });
            }
        }

        [Benchmark]
        public void OpenEditUsersTest()
        {
            foreach (var task in _tasks)
            {
                task.Start();
            }

            Task.WaitAll(_tasks);
        }
    }
}
