
using System.Collections.Generic;
using System.Threading.Tasks;

using ASC.Files.Benchmark.BenchmarkEnviroment;
using ASC.Files.Benchmark.TestsConfiguration;

using BenchmarkDotNet.Attributes;

namespace ASC.Files.Benchmark.Benchmarks
{
    [Config(typeof(DeleteFileTestConfig))]
    public class DeleteFileBenchmark
    {
        private TestDataStorage _usersStorage = TestDataStorage.GetStorage();
        private int _fileId;
        private List<int> _filesId;
        private Task[] _tasks;

        #region DeleteFileTest
        [IterationSetup(Target = nameof(DeleteFileTest))]
        public void IterSetupDeleteFileTest()
        {
            _fileId = _usersStorage.Users[0].CreateFileInMy();
        }
        
        [Benchmark]
        public void DeleteFileTest()
        {
            _usersStorage.Users[0].DeleteFile(_fileId);
        }
        #endregion

        #region DeleteFileUsersTest
        [IterationSetup(Target =nameof(DeleteFileUsersTest))]
        public void IterSetupDeleteFileUsersTest()
        {
            _filesId = new List<int>();

            foreach (var user in _usersStorage.Users)
            {
                _filesId.Add(user.CreateFileInMy());
            }

            _tasks = new Task[_usersStorage.Users.Count];

            for (int i = 0; i < _usersStorage.Users.Count; i++)
            {
                var user = _usersStorage.Users[i];
                var fileId = _filesId[i];

                _tasks[i] = new Task(() =>
                {
                    user.DeleteFile(fileId);
                });
            }
        }

        [Benchmark]
        public void DeleteFileUsersTest()
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
