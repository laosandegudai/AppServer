
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Toolchains.InProcess.Emit;

namespace ASC.Files.Benchmark.TestsConfiguration
{
    public class DeleteFavoritesTestConfig : ConfigurationBase
    {
        public DeleteFavoritesTestConfig()
        {
            AddJob(Job.Default
                .WithStrategy(BenchmarkDotNet.Engines.RunStrategy.Monitoring)
                .WithToolchain(InProcessEmitToolchain.Instance)
                .WithIterationCount(int.Parse(appConfig["Common:DeleteFavoritesTest:IterationCount"])));
        }
    }
}
