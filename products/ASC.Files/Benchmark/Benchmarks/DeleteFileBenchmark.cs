
using System.Collections.Generic;
using System.Threading.Tasks;

using ASC.Files.Benchmark.TestsConfiguration;

using BenchmarkDotNet.Attributes;

namespace ASC.Files.Benchmark.Benchmarks
{
    [Config(typeof(DeleteFileTestConfig))]
    public class DeleteFileBenchmark : BenchmarkBase
    {
        private int _fileId;
        private List<int> _filesId;
        private Task[] _tasks;

        #region DeleteFileTest
        [IterationSetup(Target = nameof(DeleteFileTest))]
        public void IterSetupDeleteFileTest()
        {
            _fileId = _dataStorage.Users[0].CreateFileInMy();
        }
        
        [Benchmark]
        public void DeleteFileTest()
        {
            _dataStorage.Users[0].DeleteFile(_fileId);
        }
        #endregion

        #region DeleteFileUsersTest
        [IterationSetup(Target =nameof(DeleteFileManyUsersTest))]
        public void IterSetupDeleteFileManyUsersTest()
        {
            _filesId = new List<int>();

            foreach (var user in _dataStorage.Users)
            {
                _filesId.Add(user.CreateFileInMy());
            }

            _tasks = new Task[_dataStorage.Users.Count];

            for (int i = 0; i < _dataStorage.Users.Count; i++)
            {
                var user = _dataStorage.Users[i];
                var fileId = _filesId[i];

                _tasks[i] = new Task(() =>
                {
                    user.DeleteFile(fileId);
                }, TaskCreationOptions.LongRunning);
                _tasks[i].ConfigureAwait(false);
            }
        }

        [Benchmark]
        public void DeleteFileManyUsersTest()
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
