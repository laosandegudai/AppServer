using System.Collections.Generic;
using System.Threading.Tasks;

using ASC.Files.Benchmark.TestsConfiguration;

using BenchmarkDotNet.Attributes;

namespace ASC.Files.Benchmark.Benchmarks
{
    [Config(typeof(DeleteFolderTestConfig))]
    public class DeleteFolderBenchmark : BenchmarkBase
    {
        private int _folderId;
        private List<int> _foldersId;
        private Task[] _tasks;

        #region DeleteFolderTest
        [IterationSetup(Target = nameof(DeleteFolderTest))]
        public void IterSetupDeleteFolderTest()
        {
            _folderId = _dataStorage.Users[0].CreateFolderInMy();
        }

        [Benchmark]
        public void DeleteFolderTest()
        {
            _dataStorage.Users[0].DeleteFolder(_folderId);
        }
        #endregion

        #region DeleteFolderManyUsersTest
        [IterationSetup(Target = nameof(DeleteFolderManyUsersTest))]
        public void IterSetupDeleteFolderManyUsersTest()
        {
            _foldersId = new List<int>();

            foreach (var user in _dataStorage.Users)
            {
                _foldersId.Add(user.CreateFolderInMy());
            }

            _tasks = new Task[_dataStorage.Users.Count];

            for (int i = 0; i < _dataStorage.Users.Count; i++)
            {
                var user = _dataStorage.Users[i];
                var folderId = _foldersId[i];

                _tasks[i] = new Task(() =>
                {
                    user.DeleteFolder(folderId);
                }, TaskCreationOptions.LongRunning);
                _tasks[i].ConfigureAwait(false);
            }
        }

        [Benchmark]
        public void DeleteFolderManyUsersTest()
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
