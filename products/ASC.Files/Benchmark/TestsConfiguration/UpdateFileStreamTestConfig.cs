
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Toolchains.InProcess.Emit;

namespace ASC.Files.Benchmark.TestsConfiguration
{
    class UpdateFileStreamTestConfig : ConfigurationBase
    {
        public UpdateFileStreamTestConfig()
        {
            AddJob(Job.Default
                .WithStrategy(BenchmarkDotNet.Engines.RunStrategy.Monitoring)
                .WithToolchain(InProcessEmitToolchain.Instance)
                .WithIterationCount(int.Parse(appConfig["Files:UpdateFileStreamTest:IterationCount"])));
            AddColumn(new TagColumn("Size", name => $"{appConfig["Files:UpdateFileStreamTest:StreamSizeKB"]} KB"));
        }
    }
}
