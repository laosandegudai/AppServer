
using BenchmarkDotNet.Attributes;

namespace ASC.Files.Benchmark
{
    [Config(typeof(MyDocumentsConfig))]
    public class MyDocumentsBenchmark
    {
        private BenchmarkFilesSut _sut = new BenchmarkFilesSut();
        private int _fileId;

        [IterationSetup(Target = nameof(CreateFileTest))]
        public void SetupCreateFile()
        {
            _sut.Setup();
        }

        [IterationSetup(Target = nameof(DeleteFileTest))]
        public void SetupDeleteFile()
        {
            _sut.Setup();
            _fileId = _sut.CreateFile();
        }

        [Benchmark]
        public void CreateFileTest()
        {
            _sut.CreateFile();
        }

        [Benchmark]
        public void DeleteFileTest()
        {
            _sut.DeleteFile(_fileId);
        }
    }
}
