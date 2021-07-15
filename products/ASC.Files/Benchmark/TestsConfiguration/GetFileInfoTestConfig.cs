
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Toolchains.InProcess.Emit;

namespace ASC.Files.Benchmark.TestsConfiguration
{
    public class GetFileInfoTestConfig : ConfigurationBase
    {
        public GetFileInfoTestConfig()
        {
            AddJob(Job.Default
                .WithStrategy(BenchmarkDotNet.Engines.RunStrategy.Monitoring)
                .WithToolchain(InProcessEmitToolchain.Instance)
                .WithIterationCount(int.Parse(appConfig["Files:GetFileInfoTest:IterationCount"])));
        }
    }
}
