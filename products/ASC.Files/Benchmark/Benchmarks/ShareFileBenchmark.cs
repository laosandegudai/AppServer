
using System.Collections.Generic;

using ASC.Files.Benchmark.TestsConfiguration;

using BenchmarkDotNet.Attributes;

namespace ASC.Files.Benchmark.Benchmarks
{
    [Config(typeof(ShareFileTestConfig))]
    public class ShareFileBenchmark : BenchmarkBase
    {
        private int _fileId;
        private List<int> _filesId;

        #region ShareFileTest
        [GlobalSetup(Target = nameof(ShareFileTest))]
        public void GlobalSetupShareFileTest()
        {
            _filesId = new List<int>();

            for (int i = 0; i < 10; i++)
            {
                _filesId.Add(_dataStorage.Users[0].CreateFileInMy());
            }
        }

        //[IterationCleanup(Target =nameof(ShareFileTest))]
        public void IterClenupShareFileTest()
        {
            _dataStorage.Users[0].RemoveShare(new List<int> { _fileId }, new List<int>());
        }
        
        [Benchmark]
        public void ShareFileTest()
        {
            foreach (var file in _filesId)
            {
                _dataStorage.Users[0].ShareFile(file, _dataStorage.Users[1].Id);
            }
        }
        #endregion    
    }
}
