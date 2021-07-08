using System.Collections.Generic;

using ASC.Files.Benchmark.BenchmarkEnviroment;
using ASC.Files.Benchmark.TestsConfiguration;

using BenchmarkDotNet.Attributes;

namespace ASC.Files.Benchmark
{
    [Config(typeof(UpdateFileStreamTestConfig))]
    public class UpdateFileStreamBenchmark
    {
        private BenchmarkFilesSut _sut = new BenchmarkFilesSut();
        private int _fileId;

        public IEnumerable<TestStream> TestStreams()
        {
            yield return StreamGenerator.Generate(1024);
            yield return StreamGenerator.Generate(2048);
            yield return StreamGenerator.Generate(4096);
        }

        [GlobalSetup(Target = nameof(UpdateFileStreamTest))]
        public void SetupUpdateFileStream()
        {
            _sut.Setup();
            _fileId = _sut.CreateFile();
        }

        [IterationSetup(Target = nameof(UpdateFileStreamTest))]
        public void IterSetupUpdateFileStream()
        {
            _sut.Setup();
        }

        [Benchmark]
        [ArgumentsSource(nameof(TestStreams))]
        public void UpdateFileStreamTest(TestStream testStream)
        {
            testStream.Stream.Position = 0;
            _sut.UpdateFileStream(_fileId, testStream.Stream);
        }
    }
}
