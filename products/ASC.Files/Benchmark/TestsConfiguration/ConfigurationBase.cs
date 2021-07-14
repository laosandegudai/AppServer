
using BenchmarkDotNet.Configs;

using Microsoft.Extensions.Configuration;

namespace ASC.Files.Benchmark.TestsConfiguration
{
    public class ConfigurationBase : ManualConfig
    {
        protected IConfigurationRoot appConfig = BenchmarkConfiguration.Build().Config;
    }
}
