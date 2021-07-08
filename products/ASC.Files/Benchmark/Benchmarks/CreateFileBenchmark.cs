
using BenchmarkDotNet.Attributes;

namespace ASC.Files.Benchmark
{
    [Config(typeof(CreateFileTestConfig))]
    public class CreateFileBenchmark
    {
        private BenchmarkFilesSut _sut = new BenchmarkFilesSut();

        [IterationSetup(Target = nameof(CreateFileTest))]
        public void IterSetupCreateFile()
        {
            _sut.Setup();
        }

        [Benchmark]
        public void CreateFileTest()
        {
            _sut.CreateFile();
        }
    }
}
