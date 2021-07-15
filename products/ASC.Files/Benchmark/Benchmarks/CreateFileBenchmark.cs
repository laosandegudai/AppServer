using System.Threading.Tasks;

using ASC.Files.Benchmark.TestsConfiguration;

using BenchmarkDotNet.Attributes;

namespace ASC.Files.Benchmark.Benchmarks
{
    [Config(typeof(CreateFileTestConfig))]
    public class CreateFileBenchmark : BenchmarkBase
    {
        private Task[] _tasks;

        #region CreateFileTest
        [Benchmark]
        public void CreateFileTest()
        {
            _dataStorage.Users[0].CreateFileInMy();
        }
        #endregion

        #region CreateFileManyUsersTest
        [IterationSetup(Target = nameof(CreateFileManyUsersTest))]
        public void IterSetupCreateFileManyUsersTest()
        {

            _tasks = new Task[_dataStorage.Users.Count];

            for (int i = 0; i < _dataStorage.Users.Count; i++)
            {
                var user = _dataStorage.Users[i];

                _tasks[i] = new Task(() =>
                {
                    user.CreateFileInMy();
                });
            }
        }

        [Benchmark]
        public void CreateFileManyUsersTest()
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
