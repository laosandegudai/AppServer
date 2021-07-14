
using System.IO;

using Microsoft.Extensions.Configuration;

namespace ASC.Files.Benchmark
{
    public class BenchmarkConfiguration
    {
        public IConfigurationRoot Config { get; private set; }

        private static BenchmarkConfiguration _instance;

        private BenchmarkConfiguration()
        {
            Config = new ConfigurationBuilder()
                .AddJsonFile(Path.GetFullPath("benchmarksettings.json"), false, false)
                .Build();
        }

        public static BenchmarkConfiguration Build()
        {
            if (_instance == null)
                _instance = new BenchmarkConfiguration();

            return _instance;
        }
    }
}
