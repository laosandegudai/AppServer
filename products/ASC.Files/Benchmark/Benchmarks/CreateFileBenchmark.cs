using System.Threading.Tasks;

using ASC.Files.Benchmark.BenchmarkEnviroment;
using ASC.Files.Benchmark.TestsConfiguration;

using BenchmarkDotNet.Attributes;

namespace ASC.Files.Benchmark.Benchmarks
{
    [Config(typeof(CreateFileTestConfig))]
    public class CreateFileBenchmark
    {
        private TestDataStorage _usersStorage = TestDataStorage.GetStorage();
        private Task[] _tasks;

        #region CreateFileTest
        [Benchmark]
        public void CreateFileTest()
        {
            _usersStorage.Users[0].CreateFileInMy();
        }
        #endregion

        #region CreateFileUsersTest
        [IterationSetup(Target = nameof(CreateFileUsersTest))]
        public void IterSetupCreateFileUsersTest()
        {

            _tasks = new Task[_usersStorage.Users.Count];

            for (int i = 0; i < _usersStorage.Users.Count; i++)
            {
                var user = _usersStorage.Users[i];

                _tasks[i] = new Task(() =>
                {
                    user.CreateFileInMy();
                });
            }
        }

        [Benchmark]
        public void CreateFileUsersTest()
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
