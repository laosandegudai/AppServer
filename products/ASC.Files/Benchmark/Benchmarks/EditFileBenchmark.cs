using System.Collections.Generic;
using System.Threading.Tasks;

using ASC.Files.Benchmark.TestsConfiguration;

using BenchmarkDotNet.Attributes;

namespace ASC.Files.Benchmark.Benchmarks
{
    [Config(typeof(EditFileTestConfig))]
    public class EditFileBenchmark : BenchmarkBase
    {
        private int _fileId;
        private List<int> _filesId = new List<int>();
        private Task[] _tasks;

        #region EditTest
        [GlobalSetup(Target = nameof(EditFileTest))]
        public void GlobalSetupEditFileTest()
        {
            _fileId = _dataStorage.Users[0].CreateFileInMy();
        }

        [Benchmark]
        public void EditFileTest()
        {
            _dataStorage.Users[0].OpenEdit(_fileId);
            _dataStorage.Users[0].StartEdit(_fileId);
            _dataStorage.Users[0].SaveEditing(_fileId);
        }
        #endregion EditTest

        #region EditManyUsersTest
        [GlobalSetup(Target =nameof(EditFileManyUsersTest))]
        public void GlobalSetupEditFileManyUsersTest()
        {
            foreach (var user in _dataStorage.Users)
            {
                _filesId.Add(user.CreateFileInMy());
            }
        }

        [IterationSetup(Target =nameof(EditFileManyUsersTest))]
        public void IterSetupEditFileManyUsersTest()
        {
            _tasks = new Task[_dataStorage.Users.Count];

            for (int i = 0; i < _dataStorage.Users.Count; i++)
            {
                var user = _dataStorage.Users[i];
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
        public void EditFileManyUsersTest()
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
