using System.IO;

using Microsoft.Extensions.Hosting;

namespace ASC.Files.Benchmark.BenchmarkEnviroment
{
    public class BenchmarkFilesHost
    {
        public IHost Host { get; }

        private const string TestConnection = "Server=localhost;Database=onlyoffice_benchmark;User ID=root;Password=onlyoffice;Pooling=true;Character Set=utf8;AutoEnlist=false;SSL Mode=none;AllowPublicKeyRetrieval=True;";

        public BenchmarkFilesHost(bool migrationEnable)
        {
            Host = Program.CreateHostBuilder(new string[] {
                "--pathToConf" , Path.Combine("..", "..", "..", "..", "..", "config"),
                "--ConnectionStrings:default:connectionString", TestConnection,
                 "--migration:enabled", migrationEnable.ToString().ToLower()}).Build();
        }
    }
}
