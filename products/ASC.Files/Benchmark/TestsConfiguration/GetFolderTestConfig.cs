
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Toolchains.InProcess.Emit;

namespace ASC.Files.Benchmark.TestsConfiguration
{
    public class GetFolderTestConfig : ConfigurationBase
    {
        public GetFolderTestConfig()
        {
            AddJob(Job.Default
                .WithStrategy(BenchmarkDotNet.Engines.RunStrategy.Monitoring)
                .WithToolchain(InProcessEmitToolchain.Instance)
                .WithIterationCount(int.Parse(appConfig["Folders:GetFolderTest:IterationCount"])));
            AddColumn(new TagColumn("FilesCount", name => appConfig["Folders:GetFolderTest:FilesInFolderCount"]));
        }
    }
}
