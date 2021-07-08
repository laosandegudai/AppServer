
using ASC.Files.Benchmark.TestsConfiguration;

using BenchmarkDotNet.Attributes;

namespace ASC.Files.Benchmark
{
    [Config(typeof(GetFileInfoTestConfig))]
    public class GetFileInfoBenchmark
    {
        private BenchmarkFilesSut _sut = new BenchmarkFilesSut();
        private int _fileId;

        [GlobalSetup(Target = nameof(GetFileInfoTest))]
        public void GlobalSetupGetFileInfo()
        {
            _sut.Setup();
            _fileId = _sut.CreateFile();
        }

        [Benchmark]
        public void GetFileInfoTest()
        {
            _sut.GetFileInfo(_fileId);
        }
    }
}
