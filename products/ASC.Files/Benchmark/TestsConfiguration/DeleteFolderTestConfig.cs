using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Toolchains.InProcess.Emit;

namespace ASC.Files.Benchmark.TestsConfiguration
{
    public class DeleteFolderTestConfig : ConfigurationBase
    {
        public DeleteFolderTestConfig()
        {
            AddJob(Job.Default
                .WithStrategy(BenchmarkDotNet.Engines.RunStrategy.Monitoring)
                .WithToolchain(InProcessEmitToolchain.Instance)
                .WithIterationCount(int.Parse(appConfig["Folders:DeleteFolderTest:IterationCount"])));
        }
    }
}
