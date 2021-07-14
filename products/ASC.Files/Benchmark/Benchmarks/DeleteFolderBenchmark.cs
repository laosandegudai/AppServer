using System.Collections.Generic;
using System.Threading.Tasks;

using ASC.Files.Benchmark.BenchmarkEnviroment;
using ASC.Files.Benchmark.TestsConfiguration;

using BenchmarkDotNet.Attributes;

namespace ASC.Files.Benchmark.Benchmarks
{
    [Config(typeof(DeleteFolderTestConfig))]
    public class DeleteFolderBenchmark
    {
        private TestDataStorage _usersStorage = TestDataStorage.GetStorage();
        private int _folderId;
        private List<int> _foldersId;
        private Task[] _tasks;

        #region DeleteFolderTest
        [IterationSetup(Target = nameof(DeleteFolderTest))]
        public void IterSetupDeleteFolderTest()
        {
            _folderId = _usersStorage.Users[0].CreateFolderInMy();
        }

        [Benchmark]
        public void DeleteFolderTest()
        {
            _usersStorage.Users[0].DeleteFolder(_folderId);
        }
        #endregion

        #region DeleteFolderUsersTest
        [IterationSetup(Target = nameof(DeleteFolderUsersTest))]
        public void IterSetupDeleteFolderUsersTest()
        {
            _foldersId = new List<int>();

            foreach (var user in _usersStorage.Users)
            {
                _foldersId.Add(user.CreateFolderInMy());
            }

            _tasks = new Task[_usersStorage.Users.Count];

            for (int i = 0; i < _usersStorage.Users.Count; i++)
            {
                var user = _usersStorage.Users[i];
                var folderId = _foldersId[i];

                _tasks[i] = new Task(() =>
                {
                    user.DeleteFolder(folderId);
                });
            }
        }

        [Benchmark]
        public void DeleteFolderUsersTest()
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
