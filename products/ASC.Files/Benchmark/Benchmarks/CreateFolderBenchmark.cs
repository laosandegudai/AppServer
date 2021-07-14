
using System.Threading.Tasks;

using ASC.Files.Benchmark.BenchmarkEnviroment;
using ASC.Files.Benchmark.TestsConfiguration;

using BenchmarkDotNet.Attributes;

namespace ASC.Files.Benchmark.Benchmarks
{
    [Config(typeof(CreateFolderTestConfig))]
    public class CreateFolderBenchmark
    {
        private TestDataStorage _usersStorage = TestDataStorage.GetStorage();
        private Task[] _tasks;

        #region CreateFolderTest
        [Benchmark]
        public void CreateFolderTest()
        {
            _usersStorage.Users[0].CreateFolderInMy();
        }
        #endregion

        #region CreateFolderUsersTest
        [IterationSetup(Target = nameof(CreateFolderUsersTest))]
        public void IterSetupCreateFolderUsersTest()
        {

            _tasks = new Task[_usersStorage.Users.Count];

            for (int i = 0; i < _usersStorage.Users.Count; i++)
            {
                var user = _usersStorage.Users[i];

                _tasks[i] = new Task(() =>
                {
                    user.CreateFolderInMy();
                });
            }
        }

        [Benchmark]
        public void CreateFolderUsersTest()
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
