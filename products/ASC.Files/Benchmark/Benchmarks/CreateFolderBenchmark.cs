using System.Threading.Tasks;

using ASC.Files.Benchmark.TestsConfiguration;

using BenchmarkDotNet.Attributes;

namespace ASC.Files.Benchmark.Benchmarks
{
    [Config(typeof(CreateFolderTestConfig))]
    public class CreateFolderBenchmark : BenchmarkBase
    {
        private Task[] _tasks;

        #region CreateFolderTest
        [Benchmark]
        public void CreateFolderTest()
        {
            _dataStorage.Users[0].CreateFolderInMy();
        }
        #endregion

        #region CreateFolderManyUsersTest
        [IterationSetup(Target = nameof(CreateFolderManyUsersTest))]
        public void IterSetupCreateFolderManyUsersTest()
        {

            _tasks = new Task[_dataStorage.Users.Count];

            for (int i = 0; i < _dataStorage.Users.Count; i++)
            {
                var user = _dataStorage.Users[i];

                _tasks[i] = new Task(() =>
                {
                    user.CreateFolderInMy();
                });
            }
        }

        [Benchmark]
        public void CreateFolderManyUsersTest()
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
