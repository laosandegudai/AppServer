using System.Collections.Generic;
using System.Threading.Tasks;

using ASC.Files.Benchmark.TestsConfiguration;

using BenchmarkDotNet.Attributes;

namespace ASC.Files.Benchmark.Benchmarks
{
    [Config(typeof(GetFolderTestConfig))]
    public class GetFolderBenchmark : BenchmarkBase
    {
        private int _folderId;
        private List<int> _foldersId = new List<int>();
        private Task[] _tasks;
        private int _filesCount;

        public GetFolderBenchmark()
        {
            _filesCount = int.Parse(_config["Folders:GetFolderTest:FilesInFolderCount"]);
        }

        #region GetFolderTest
        [GlobalSetup(Target = nameof(GetFolderTest))]
        public void GlobalSetupGetFolderTest()
        {
            _folderId = _dataStorage.Users[0].CreateFolderInMy();
            
            for (int i = 0; i < _filesCount; i++)
            {
                _dataStorage.Users[0].CreateFile(_folderId);
            }
        }

        [Benchmark]
        public void GetFolderTest()
        {
            _dataStorage.Users[0].GetFolder(_folderId);
        }
        #endregion

        #region GetFolderManyUsersTest
        [GlobalSetup(Target = nameof(GetFolderManyUsersTest))]
        public void GlobalSetupGetFolderManyUsersTest()
        {
            foreach (var user in _dataStorage.Users)
            {
                var folderId = user.CreateFolderInMy();

                for (int i = 0; i < _filesCount; i++)
                {
                    user.CreateFile(folderId);
                }

                _foldersId.Add(folderId);
            }
        }

        [IterationSetup(Target = nameof(GetFolderManyUsersTest))]
        public void IterSetupGetFolderManyUsersTest()
        {
            _tasks = new Task[_dataStorage.Users.Count];

            for (int i = 0; i < _dataStorage.Users.Count; i++)
            {
                var user = _dataStorage.Users[i];
                var folderId = _foldersId[i];

                _tasks[i] = new Task(() =>
                {
                    user.GetFolder(folderId);
                }, TaskCreationOptions.LongRunning);
                _tasks[i].ConfigureAwait(false);
            }
        }

        [Benchmark]
        public void GetFolderManyUsersTest()
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
