using System.Collections.Generic;
using System.Threading.Tasks;

using ASC.Files.Benchmark.BenchmarkEnviroment;
using ASC.Files.Benchmark.TestsConfiguration;

using BenchmarkDotNet.Attributes;

namespace ASC.Files.Benchmark.Benchmarks
{
    [Config(typeof(GetFileInfoTestConfig))]
    public class GetFileInfoBenchmark
    {
        private TestDataStorage _usersStorage = TestDataStorage.GetStorage();
        private int _fileId;
        private List<int> _filesId = new List<int>();
        private Task[] _tasks;

        #region GetFileInfoTest
        [GlobalSetup(Target = nameof(GetFileInfoTest))]
        public void GlobalSetupGetFileInfoTest()
        {
            _fileId = _usersStorage.Users[0].CreateFileInMy();
        }

        [Benchmark]
        public void GetFileInfoTest()
        {
            _usersStorage.Users[0].GetFileInfo(_fileId);
        }
        #endregion

        #region GetFileInfoUsersTest
        [GlobalSetup(Target = nameof(GetFileInfoUsersTest))]
        public void GlobalSetupGetFileInfoUsersTest()
        {
            foreach (var user in _usersStorage.Users)
            {
                _filesId.Add(user.CreateFileInMy());
            }
        }

        [IterationSetup(Target = nameof(GetFileInfoUsersTest))]
        public void IterSetupGetFileInfoUsersTest()
        {
            _tasks = new Task[_usersStorage.Users.Count];

            for (int i = 0; i < _usersStorage.Users.Count; i++)
            {
                var user = _usersStorage.Users[i];
                var fileId = _filesId[i];

                _tasks[i] = new Task(() =>
                {
                    user.GetFileInfo(fileId);
                });
            }
        }

        [Benchmark]
        public void GetFileInfoUsersTest()
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
