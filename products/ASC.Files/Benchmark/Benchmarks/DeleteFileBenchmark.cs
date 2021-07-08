
using ASC.Files.Benchmark.TestsConfiguration;

using BenchmarkDotNet.Attributes;

namespace ASC.Files.Benchmark
{
    [Config(typeof(DeleteFileTestConfig))]
    public class DeleteFileBenchmark
    {
        private BenchmarkFilesSut _sut = new BenchmarkFilesSut();
        private int _fileId;

        [IterationSetup(Target = nameof(DeleteFileTest))]
        public void IterSetupDeleteFile()
        {
            _sut.Setup();
            _fileId = _sut.CreateFile();
        }

        [Benchmark]
        public void DeleteFileTest()
        {
            _sut.DeleteFile(_fileId);
        }
    }
}
