
using System.Collections.Generic;
using System.Threading.Tasks;

using ASC.Files.Benchmark.TestsConfiguration;

using BenchmarkDotNet.Attributes;

namespace ASC.Files.Benchmark.Benchmarks
{
    [Config(typeof(ShareFileTestConfig))]
    public class ShareFileBenchmark : BenchmarkBase
    {
        private int _fileId;
        private List<int> _filesId;
        private int _filesCount;
        private Task[] _tasks;
        
        public ShareFileBenchmark()
        {
            _filesCount = int.Parse(_config["Files:ShareFileTest:FilesCount"]);
        }

        #region ShareFileTest
        [GlobalSetup(Target = nameof(ShareFileTest))]
        public void GlobalSetupShareFileTest()
        {
            _fileId = _dataStorage.Users[0].CreateFileInMy();
        }
        
        [Benchmark]
        public void ShareFileTest()
        {
            _dataStorage.Users[0].ShareFile(_fileId, _dataStorage.Users[1].Id);
        }
        #endregion

        #region GetShareFolderWithFiles
        [GlobalSetup(Target =nameof(GetShareFolderWithFiles))]
        public void GlobalSetupGetShareFolderWithFiles()
        {
            for (int i = 0; i < _filesCount; i++)
            {
                var id = _dataStorage.Users[0].CreateFileInMy();
                _dataStorage.Users[0].ShareFile(id, _dataStorage.Users[1].Id);
            }
        }

        [Benchmark]
        public void GetShareFolderWithFiles()
        {
            _dataStorage.Users[1].GetShareFolder();
        }
        #endregion

        #region ShareFileManyUsersTest
        [GlobalSetup(Target =nameof(ShareFileManyUsersTest))]
        public void GlobalSetupShareFileManyUsersTest()
        {
            _filesId = new List<int>();

            foreach (var user in _dataStorage.Users)
            {
                _filesId.Add(user.CreateFileInMy());
            }
        }

        [IterationSetup(Target =nameof(ShareFileManyUsersTest))]
        public void IterSetupShareFileManyUsersTest()
        {
            _tasks = new Task[_dataStorage.Users.Count];

            for (int i = 0; i < _dataStorage.Users.Count; i++)
            {
                var user1 = _dataStorage.Users[i];
                var user2 = _dataStorage.UsersForShare[i];
                var fileId = _filesId[i];

                _tasks[i] = new Task(() =>
                {
                    user1.ShareFile(fileId, user2.Id);
                }, TaskCreationOptions.LongRunning);
                _tasks[i].ConfigureAwait(false);
            }
        }

        [Benchmark]
        public void ShareFileManyUsersTest()
        {
            foreach (var task in _tasks)
            {
                task.Start();
            }

            Task.WaitAll(_tasks);
        }
        #endregion

        #region GetShareFolderWithFilesManyUsersTest
        [GlobalSetup(Target = nameof(GetShareFolderWithFilesManyUsersTest))]
        public void GlobalSetupGetShareFolderWithFilesManyUsersTest()
        {
            _filesId = new List<int>();

            for (int i = 0; i < _filesCount; i++)
            {
                _filesId.Add(_dataStorage.Users[0].CreateFileInMy());
            }

            foreach (var user in _dataStorage.UsersForShare)
            {
                foreach (var file in _filesId)
                {
                    _dataStorage.Users[0].ShareFile(file, user.Id);
                }
            }
        }

        [IterationSetup(Target =nameof(GetShareFolderWithFilesManyUsersTest))]
        public void IterSetupGetShareFolderManyUsersTest()
        {
            _tasks = new Task[_dataStorage.UsersForShare.Count];

            for (int i = 0; i < _dataStorage.UsersForShare.Count; i++)
            {
                var user = _dataStorage.UsersForShare[i];

                _tasks[i] = new Task(() =>
                {
                    user.GetShareFolder();
                }, TaskCreationOptions.LongRunning);
                _tasks[i].ConfigureAwait(false);
            }
        }

        [Benchmark]
        public void GetShareFolderWithFilesManyUsersTest()
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
