using ASC.Files.Benchmark.BenchmarkEnviroment;

using BenchmarkDotNet.Running;

namespace Benchmark
{
    public class Program
    {
        static void Main(string[] args)
        {
            using var enviroment = new BenchmarkTestEnviroment();
            enviroment.Create();

            BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);
        }
    }
}
