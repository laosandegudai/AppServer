using System.Collections.Generic;
using System.Threading.Tasks;

using ASC.Files.Benchmark.TestsConfiguration;

using BenchmarkDotNet.Attributes;

namespace ASC.Files.Benchmark.Benchmarks
{
    [Config(typeof(AddToFavoritesTestConfig))]
    public class AddToFavoritesBenchmark : BenchmarkBase
    {
        private List<int> _filesId;
        private List<int> _foldersId;
        private List<List<int>> _filesManyUsersId;
        private List<List<int>> _foldersManyUsersId;
        private Task[] _tasks;
        private int _filesCount;
        private int _foldersCount;

        public AddToFavoritesBenchmark()
        {
            _filesCount = int.Parse(_config["Common:AddToFavoritesTest:FilesFavoriteCountPerUser"]);
            _foldersCount = int.Parse(_config["Common:AddToFavoritesTest:FoldersFavoriteCountPerUser"]);
        }

        [GlobalSetup(Targets = new string[] { nameof(AddToFavoritesOnlyFilesTest),
            nameof(AddToFavoritesOnlyFoldersTest), nameof(AddToFavoritesFullTest) })]
        public void GlobaSetupAddToFavoritesTest()
        {
            _filesId = new List<int>();
            _foldersId = new List<int>();

            for (int i = 0; i < _filesCount; i++)
            {
                _filesId.Add(_dataStorage.Users[0].CreateFileInMy());  
            }

            for (int i = 0; i < _foldersCount; i++)
            {
                _foldersId.Add(_dataStorage.Users[0].CreateFolderInMy());
            }
        }

        [Benchmark]
        public void AddToFavoritesOnlyFilesTest()
        {
            _dataStorage.Users[0].AddToFavorites(new List<int>(), _filesId);
        }

        [Benchmark]
        public void AddToFavoritesOnlyFoldersTest()
        {
            _dataStorage.Users[0].AddToFavorites(_foldersId, new List<int>());
        }

        [Benchmark]
        public void AddToFavoritesFullTest()
        {
            _dataStorage.Users[0].AddToFavorites(_foldersId, _filesId);
        }

        [GlobalSetup(Target = nameof(AddToFavoritesOnlyFilesManyUsersTest))]
        public void GlobalSetupAddToFavoritesOnlyFilesManyUsersTest()
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

        [IterationSetup(Target = nameof(AddToFavoritesOnlyFilesManyUsersTest))]
        public void IterSetupAddToFavoritesOnlyFilesManyUsersTest()
        {
            _tasks = new Task[_dataStorage.Users.Count];

            for (int i = 0; i < _dataStorage.Users.Count; i++)
            {
                var filesId = _filesManyUsersId[i];
                var user = _dataStorage.Users[i];

                _tasks[i] = new Task(() =>
                {
                    user.AddToFavorites(new List<int>(), filesId);
                });
            }
        }

        [Benchmark]
        public void AddToFavoritesOnlyFilesManyUsersTest()
        {
            foreach (var task in _tasks)
            {
                task.Start();
            }

            Task.WaitAll(_tasks);
        }

        [GlobalSetup(Target = nameof(AddToFavoritesOnlyFoldersUsersTest))]
        public void GlobalSetupAddToFavoritesOnlyFoldersManyUsersTest()
        {
            _foldersManyUsersId = new List<List<int>>();

            foreach (var user in _dataStorage.Users)
            {
                var foldersId = new List<int>();

                for (int i = 0; i < _foldersCount; i++)
                {
                    foldersId.Add(user.CreateFolderInMy());
                }

                _foldersManyUsersId.Add(foldersId);
            }
        }

        [IterationSetup(Target = nameof(AddToFavoritesOnlyFoldersUsersTest))]
        public void IterSetupAddToFavoritesOnlyFoldersManyUsersTest()
        {
            _tasks = new Task[_dataStorage.Users.Count];

            for (int i = 0; i < _dataStorage.Users.Count; i++)
            {
                var foldersId = _foldersManyUsersId[i];
                var user = _dataStorage.Users[i];

                _tasks[i] = new Task(() =>
                {
                    user.AddToFavorites(foldersId, new List<int>());
                });
            }
        }

        [Benchmark]
        public void AddToFavoritesOnlyFoldersUsersTest()
        {
            foreach (var task in _tasks)
            {
                task.Start();
            }

            Task.WaitAll(_tasks);
        }
    }
}
