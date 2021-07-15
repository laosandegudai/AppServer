using System.Collections.Generic;
using System.Threading.Tasks;

using ASC.Files.Benchmark.BenchmarkEnviroment;
using ASC.Files.Benchmark.TestsConfiguration;

using BenchmarkDotNet.Attributes;

namespace ASC.Files.Benchmark.Benchmarks
{
    [Config(typeof(StartEditTestConfig))]
    public class EditFileBenchmark
    {
        private TestDataStorage _usersStorage = TestDataStorage.GetStorage();
        private int _fileId;
        private List<int> _filesId = new List<int>();
        private Task[] _tasks;

        #region EditTest
        [GlobalSetup(Target = nameof(EditTest))]
        public void GlobalSetupEditTest()
        {
            _fileId = _usersStorage.Users[0].CreateFileInMy();
        }

        [Benchmark]
        public void EditTest()
        {
            _usersStorage.Users[0].OpenEdit(_fileId);
            _usersStorage.Users[0].StartEdit(_fileId);
            _usersStorage.Users[0].SaveEditing(_fileId);
        }
        #endregion EditTest

        #region EditUsersTest
        [GlobalSetup(Target =nameof(EditUsersTest))]
        public void GlobalSetupEditUsersTest()
        {
            foreach (var user in _usersStorage.Users)
            {
                _filesId.Add(user.CreateFileInMy());
            }
        }

        [IterationSetup(Target =nameof(EditUsersTest))]
        public void IterSetupEditUsersTest()
        {
            _tasks = new Task[_usersStorage.Users.Count];

            for (int i = 0; i < _usersStorage.Users.Count; i++)
            {
                var user = _usersStorage.Users[i];
                var fileId = _filesId[i];

                _tasks[i] = new Task(() =>
                {
                    user.OpenEdit(fileId);
                    user.StartEdit(fileId);
                    user.SaveEditing(fileId);
                });
            }
        }

        [Benchmark]
        public void EditUsersTest()
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
