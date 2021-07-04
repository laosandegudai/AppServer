using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Mathematics;
using BenchmarkDotNet.Toolchains.InProcess;
using BenchmarkDotNet.Toolchains.InProcess.Emit;

namespace ASC.Files.Benchmark
{
    public class MyDocumentsConfig : ManualConfig
    {
        public MyDocumentsConfig()
        {
            AddJob(Job.Default
                .WithToolchain(InProcessEmitToolchain.Instance)
                .WithIterationCount(100))
                .AddColumn(StatisticColumn.P0)
                .AddColumn(StatisticColumn.P25)
                .AddColumn(StatisticColumn.P50)
                .AddColumn(StatisticColumn.P67)
                .AddColumn(StatisticColumn.P80)
                .AddColumn(StatisticColumn.P85)
                .AddColumn(StatisticColumn.P90)
                .AddColumn(StatisticColumn.P95)
                .AddColumn(StatisticColumn.P100);
        }
    }
}
