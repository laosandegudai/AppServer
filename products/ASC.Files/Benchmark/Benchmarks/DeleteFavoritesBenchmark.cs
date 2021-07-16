using System.Collections.Generic;
using System.Threading.Tasks;

using ASC.Files.Benchmark.TestsConfiguration;

using BenchmarkDotNet.Attributes;

namespace ASC.Files.Benchmark.Benchmarks
{
    [Config(typeof(DeleteFavoritesTestConfig))]
    public class DeleteFavoritesBenchmark : BenchmarkBase
    {
        private List<int> _filesId;
        private List<int> _foldersId;
        private List<List<int>> _filesManyUsersId;
        private List<List<int>> _foldersManyUsersId;
        private Task[] _tasks;
        private int _filesCount;
        private int _foldersCount;

        public DeleteFavoritesBenchmark()
        {
            _filesCount = int.Parse(_config["Common:DeleteFavoritesTest:FilesFavoriteCountPerUser"]);
            _foldersCount = int.Parse(_config["Common:DeleteFavoritesTest:FoldersFavoriteCountPerUser"]);
        }

        #region DeleteFavoritesOnlyFilesTest
        [GlobalSetup(Target = nameof(DeleteFavoritesOnlyFilesTest))]
        public void GlobalSetupDeleteFavoritesOnlyFilesTest()
        {
            _filesId = new List<int>();
            
            for (int i = 0; i < _filesCount; i++)
            {
                _filesId.Add(_dataStorage.Users[0].CreateFileInMy());
            }
        }

        [IterationSetup(Target =nameof(DeleteFavoritesOnlyFilesTest))]
        public void IterSetupDeleteFavoritesOnlyFilesTest()
        {
            _dataStorage.Users[0].AddToFavorites(new List<int>(), _filesId);
        }

        [Benchmark]
        public void DeleteFavoritesOnlyFilesTest()
        {
            _dataStorage.Users[0].DeleteFavorites(new List<int>(), _filesId);
        }
        #endregion

        #region DeleteFavoritesOnlyFoldersTest
        [GlobalSetup(Target = nameof(DeleteFavoritesOnlyFoldersTest))]
        public void GlobalSetupDeleteFavoritesOnlyFoldersTest()
        {
            _foldersId = new List<int>();

            for (int i = 0; i < _foldersCount; i++)
            {
                _foldersId.Add(_dataStorage.Users[0].CreateFolderInMy());
            }
        }

        [IterationSetup(Target =nameof(DeleteFavoritesOnlyFoldersTest))]
        public void IterSetupDeleteFavoritesOnlyFoldersTest()
        {
            _dataStorage.Users[0].AddToFavorites(_foldersId, new List<int>());
        }

        [Benchmark]
        public void DeleteFavoritesOnlyFoldersTest()
        {
            _dataStorage.Users[0].DeleteFavorites(_foldersId, new List<int>());
        }
        #endregion

        #region DeleteFavoritesFullTest
        [GlobalSetup(Target =nameof(DeleteFavoritesFullTest))]
        public void GlobalSetupDeleteFavoritesFullTest()
        {
            GlobalSetupDeleteFavoritesOnlyFilesTest();
            GlobalSetupDeleteFavoritesOnlyFoldersTest();
        }

        [IterationSetup(Target =nameof(DeleteFavoritesFullTest))]
        public void IterSetupDeleteFavoritesFullTest()
        {
            IterSetupDeleteFavoritesOnlyFilesTest();
            IterSetupDeleteFavoritesOnlyFoldersTest();
        }

        [Benchmark]
        public void DeleteFavoritesFullTest()
        {
            _dataStorage.Users[0].DeleteFavorites(_foldersId, _filesId);
        }
        #endregion

        #region DeleteFavoritesOnlyFilesManyUsersTest
        [GlobalSetup(Target =nameof(DeleteFavoritesOnlyFilesManyUsersTest))]
        public void GlobalSetupDeleteFavoritestOnlyFilesManyUsersTest()
        {
            _filesManyUsersId = new List<List<int>>();

            foreach (var user in _dataStorage.Users)
            {
                var filesId = new List<int>();

                for (int i = 0; i < _filesCount; i++)
                {
                    filesId.Add(user.CreateFileInMy());
                }

                _filesManyUsersId.Add(filesId);
            }
        }

        [IterationSetup(Target =nameof(DeleteFavoritesOnlyFilesManyUsersTest))]
        public void IterSetupDeleteFavoritestOnlyFilesManyUsersTest()
        {
            _tasks = new Task[_dataStorage.Users.Count];

            for (int i = 0; i < _dataStorage.Users.Count; i++)
            {
                var filesId = _filesManyUsersId[i];
                var user = _dataStorage.Users[i];

                _tasks[i] = new Task(() =>
                {
                    user.DeleteFavorites(new List<int>(), filesId);
                });
            }
        }

        [Benchmark]
        public void DeleteFavoritesOnlyFilesManyUsersTest()
        {
            foreach (var task in _tasks)
            {
                task.Start();
            }

            Task.WaitAll(_tasks);
        }
        #endregion

        #region DeleteFavoritesOnlyFoldersManyUsersTest
        [GlobalSetup(Target =nameof(DeleteFavoritesOnlyFoldersManyUsersTest))]
        public void GlobalSetupDeleteFavoritesOnlyFoldersManyUsersTest()
        {
            _foldersManyUsersId = new List<List<int>>();

            foreach (var user in _dataStorage.Users)
            {
                var foldersId = new List<int>();

                for (int i = 0; i < _filesCount; i++)
                {
                    foldersId.Add(user.CreateFolderInMy());
                }

                _foldersManyUsersId.Add(foldersId);
            }
        }

        [IterationSetup(Target =nameof(DeleteFavoritesOnlyFoldersManyUsersTest))]
        public void IterSetupDeleteFavoritesOnlyFoldersManyUsersTest()
        {
            _tasks = new Task[_dataStorage.Users.Count];

            for (int i = 0; i < _dataStorage.Users.Count; i++)
            {
                var foldersId = _foldersManyUsersId[i];
                var user = _dataStorage.Users[i];

                _tasks[i] = new Task(() =>
                {
                    user.DeleteFavorites(foldersId, new List<int>());
                });
            }
        }

        [Benchmark]
        public void DeleteFavoritesOnlyFoldersManyUsersTest()
        {
            foreach (var task in _tasks)
            {
                task.Start();
            }

            Task.WaitAll(_tasks);
        }
        #endregion

        #region DeleteFavoritesFullManyUsersTest
        [GlobalSetup(Target =nameof(DeleteFavoritesFullManyUsersTest))]
        public void GlobalSetupDeleteFavoritesFullManyUsersTest()
        {
            GlobalSetupDeleteFavoritestOnlyFilesManyUsersTest();
            GlobalSetupDeleteFavoritesOnlyFoldersManyUsersTest();
        }

        [IterationSetup(Target =nameof(DeleteFavoritesFullManyUsersTest))]
        public void IterSetupDeleteFavoritesFullManyUsersTest()
        {
            _tasks = new Task[_dataStorage.Users.Count];

            for (int i = 0; i < _dataStorage.Users.Count; i++)
            {
                var foldersId = _foldersManyUsersId[i];
                var filesId = _filesManyUsersId[i];
                var user = _dataStorage.Users[i];

                _tasks[i] = new Task(() =>
                {
                    user.DeleteFavorites(foldersId, filesId);
                });
            }
        }

        [Benchmark]
        public void DeleteFavoritesFullManyUsersTest()
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
