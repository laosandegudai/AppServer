using System;

using ASC.Files.Benchmark;

using BenchmarkDotNet.Running;

namespace Benchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            using var db = new BenchmarkDb();
            db.Create();

            var summary = BenchmarkRunner.Run<MyDocumentsBenchmark>();
            Console.WriteLine(summary);
        }
    }
}
