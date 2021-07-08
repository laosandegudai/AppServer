using System.IO;

using BenchmarkDotNet.Configs;

using Microsoft.Extensions.Configuration;

namespace ASC.Files.Benchmark.TestsConfiguration
{
    public class ConfigurationBase : ManualConfig
    {
        protected IConfigurationRoot appConfig = new ConfigurationBuilder()
                .AddJsonFile(Path.GetFullPath("benchmarksettings.json"), false, false)
                .Build();
    }
}
