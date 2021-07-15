using System.Collections.Generic;
using System.Threading.Tasks;

using ASC.Files.Benchmark.BenchmarkEnviroment;
using ASC.Files.Benchmark.TestsConfiguration;

using BenchmarkDotNet.Attributes;

namespace ASC.Files.Benchmark.Benchmarks
{
    [Config(typeof(AddToFavoritesTestConfig))]
    public class AddToFavoritesBenchmark
    {
        private TestDataStorage _usersStorage = TestDataStorage.GetStorage();
        private List<int> _filesId;
        private List<int> _foldersId;
        private List<List<int>> _filesUsersId;
        private List<List<int>> _foldersUsersId;
        private Task[] _tasks;

        [GlobalSetup(Targets = new string[] { nameof(SingleAddToFavoritesOnlyFilesTest),
            nameof(SingleAddToFavoritesOnlyFoldersTest), nameof(SingleAddToFavoritesFullTest) })]
        public void GlobaSetupAddToFavoritesTest()
        {
            _filesId = new List<int>();
            _foldersId = new List<int>();

            for (int i = 0; i < 10; i++)
            {
                _filesId.Add(_usersStorage.Users[0].CreateFileInMy());
                _foldersId.Add(_usersStorage.Users[0].CreateFolderInMy());
            }
        }

        [Benchmark]
        public void SingleAddToFavoritesOnlyFilesTest()
        {
            _usersStorage.Users[0].AddToFavorites(new List<int>(), _filesId);
        }

        [Benchmark]
        public void SingleAddToFavoritesOnlyFoldersTest()
        {
            _usersStorage.Users[0].AddToFavorites(_foldersId, new List<int>());
        }

        [Benchmark]
        public void SingleAddToFavoritesFullTest()
        {
            _usersStorage.Users[0].AddToFavorites(_foldersId, _filesId);
        }

        [GlobalSetup(Target = nameof(AddToFavoritesOnlyFilesUsersTest))]
        public void GlobalSetupAddToFavoritesOnlyFilesUsersTest()
        {
            _filesUsersId = new List<List<int>>();

            foreach (var user in _usersStorage.Users)
            {
                var filesId = new List<int>();
                
                for (int i = 0; i < 10; i++)
                {
                    filesId.Add(user.CreateFileInMy());
                }

                _filesUsersId.Add(filesId);
            }
        }

        [IterationSetup(Target = nameof(AddToFavoritesOnlyFilesUsersTest))]
        public void IterSetupAddToFavoritesOnlyFilesUsersTest()
        {
            _tasks = new Task[_usersStorage.Users.Count];

            for (int i = 0; i < _usersStorage.Users.Count; i++)
            {
                var filesId = _filesUsersId[i];
                var user = _usersStorage.Users[i];

                _tasks[i] = new Task(() =>
                {
                    user.AddToFavorites(new List<int>(), filesId);
                });
            }
        }

        [Benchmark]
        public void AddToFavoritesOnlyFilesUsersTest()
        {
            foreach (var task in _tasks)
            {
                task.Start();
            }

            Task.WaitAll(_tasks);
        }

        [GlobalSetup(Target = nameof(AddToFavoritesOnlyFoldersUsersTest))]
        public void GlobalSetupAddToFavoritesOnlyFoldersUsersTest()
        {
            _foldersUsersId = new List<List<int>>();

            foreach (var user in _usersStorage.Users)
            {
                var foldersId = new List<int>();

                for (int i = 0; i < 10; i++)
                {
                    foldersId.Add(user.CreateFolderInMy());
                }

                _foldersUsersId.Add(foldersId);
            }
        }

        [IterationSetup(Target = nameof(AddToFavoritesOnlyFoldersUsersTest))]
        public void IterSetupAddToFavoritesOnlyFoldersUsersTest()
        {
            _tasks = new Task[_usersStorage.Users.Count];

            for (int i = 0; i < _usersStorage.Users.Count; i++)
            {
                var foldersId = _foldersUsersId[i];
                var user = _usersStorage.Users[i];

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
