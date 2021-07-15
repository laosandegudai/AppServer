
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Toolchains.InProcess.Emit;

namespace ASC.Files.Benchmark.TestsConfiguration
{
    public class DeleteFileTestConfig : ConfigurationBase
    {
        public DeleteFileTestConfig()
        {
            AddJob(Job.Default
                .WithStrategy(BenchmarkDotNet.Engines.RunStrategy.Monitoring)
                .WithToolchain(InProcessEmitToolchain.Instance)
                .WithIterationCount(int.Parse(appConfig["Files:DeleteFileTest:IterationCount"])));
        }
    }
}
