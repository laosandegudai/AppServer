using ASC.Files.Benchmark.BenchmarkEnviroment;

using Microsoft.Extensions.Configuration;

namespace ASC.Files.Benchmark.Benchmarks
{
    public class BenchmarkBase
    {
        protected IConfigurationRoot _config  = BenchmarkConfiguration.Build().Config;
        protected TestDataStorage _dataStorage = TestDataStorage.GetStorage();
    }
}
