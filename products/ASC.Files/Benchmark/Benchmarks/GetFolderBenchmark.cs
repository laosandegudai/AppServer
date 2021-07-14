using System.Collections.Generic;
using System.Threading.Tasks;

using ASC.Files.Benchmark.BenchmarkEnviroment;
using ASC.Files.Benchmark.TestsConfiguration;

using BenchmarkDotNet.Attributes;

namespace ASC.Files.Benchmark.Benchmarks
{
    [Config(typeof(GetFolderTestConfig))]
    public class GetFolderBenchmark
    {
        private TestDataStorage _usersStorage = TestDataStorage.GetStorage();
        private int _folderId;
        private List<int> _foldersId = new List<int>();
        private Task[] _tasks;

        #region GetFolderTest
        [GlobalSetup(Target = nameof(GetFolderTest))]
        public void GlobalSetupGetFolderTest()
        {
            _folderId = _usersStorage.Users[0].CreateFolderInMy();
        }

        [Benchmark]
        public void GetFolderTest()
        {
            _usersStorage.Users[0].GetFolder(_folderId);
        }
        #endregion

        #region GetFolderUsersTest
        [GlobalSetup(Target = nameof(GetFolderUsersTest))]
        public void GlobalSetupGetFolderUsersTest()
        {
            foreach (var user in _usersStorage.Users)
            {
                _foldersId.Add(user.CreateFolderInMy());
            }
        }

        [IterationSetup(Target = nameof(GetFolderUsersTest))]
        public void IterSetupGetFolderUsersTest()
        {
            _tasks = new Task[_usersStorage.Users.Count];

            for (int i = 0; i < _usersStorage.Users.Count; i++)
            {
                var user = _usersStorage.Users[i];
                var folderId = _foldersId[i];

                _tasks[i] = new Task(() =>
                {
                    user.GetFolder(folderId);
                });
            }
        }

        [Benchmark]
        public void GetFolderUsersTest()
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
