using System.Collections.Generic;
using System.Threading.Tasks;

using ASC.Files.Benchmark.BenchmarkEnviroment;
using ASC.Files.Benchmark.TestsConfiguration;

using BenchmarkDotNet.Attributes;

namespace ASC.Files.Benchmark.Benchmarks
{
    [Config(typeof(GetFileInfoTestConfig))]
    public class GetFileInfoBenchmark : BenchmarkBase
    {
        private int _fileId;
        private List<int> _filesId = new List<int>();
        private Task[] _tasks;

        #region GetFileInfoTest
        [GlobalSetup(Target = nameof(GetFileInfoTest))]
        public void GlobalSetupGetFileInfoTest()
        {
            _fileId = _dataStorage.Users[0].CreateFileInMy();
        }

        [Benchmark]
        public void GetFileInfoTest()
        {
            _dataStorage.Users[0].GetFileInfo(_fileId);
        }
        #endregion

        #region GetFileInfoManyUsersTest
        [GlobalSetup(Target = nameof(GetFileInfoManyUsersTest))]
        public void GlobalSetupGetFileInfoManyUsersTest()
        {
            foreach (var user in _dataStorage.Users)
            {
                _filesId.Add(user.CreateFileInMy());
            }
        }

        [IterationSetup(Target = nameof(GetFileInfoManyUsersTest))]
        public void IterSetupGetFileInfoManyUsersTest()
        {
            _tasks = new Task[_dataStorage.Users.Count];

            for (int i = 0; i < _dataStorage.Users.Count; i++)
            {
                var user = _dataStorage.Users[i];
                var fileId = _filesId[i];

                _tasks[i] = new Task(() =>
                {
                    user.GetFileInfo(fileId);
                }, TaskCreationOptions.LongRunning);
                _tasks[i].ConfigureAwait(false);
            }
        }

        [Benchmark]
        public void GetFileInfoManyUsersTest()
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
