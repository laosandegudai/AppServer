using ASC.Files.Benchmark.BenchmarkEnviroment;
using ASC.Files.Benchmark.TestsConfiguration;

using BenchmarkDotNet.Attributes;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ASC.Files.Benchmark.Benchmarks
{
    [Config(typeof(RenameFolderTestConfig))]
    public class RenameFolderBenchmark
    {
        private TestDataStorage _usersStorage = TestDataStorage.GetStorage();
        private int _folderId;
        private List<int> _foldersId = new List<int>();
        private Task[] _tasks;

        #region RenameFolderTest
        [GlobalSetup(Target = nameof(RenameFolderTest))]
        public void GlobalSetupRenameFolderTest()
        {
            _folderId = _usersStorage.Users[0].CreateFolderInMy();
        }

        [Benchmark]
        public void RenameFolderTest()
        {
            _usersStorage.Users[0].RenameFolder(_folderId, "NewTestTitle");
        }
        #endregion

        #region RenameFolderUsersTest
        [GlobalSetup(Target = nameof(RenameFolderUsersTest))]
        public void GlobalSetupRenameFolderUsersTest()
        {
            foreach (var user in _usersStorage.Users)
            {
                _foldersId.Add(user.CreateFolderInMy());
            }
        }

        [IterationSetup(Target = nameof(RenameFolderUsersTest))]
        public void IterSetupRenameFolderUsersTest()
        {
            _tasks = new Task[_usersStorage.Users.Count];

            for (int i = 0; i < _usersStorage.Users.Count; i++)
            {
                var user = _usersStorage.Users[i];
                var fileId = _foldersId[i];

                _tasks[i] = new Task(() =>
                {
                    user.RenameFolder(fileId, "NewTestTitle");
                });
            }
        }

        [Benchmark]
        public void RenameFolderUsersTest()
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
