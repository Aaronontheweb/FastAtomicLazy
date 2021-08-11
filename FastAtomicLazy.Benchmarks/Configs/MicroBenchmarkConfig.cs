using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Exporters;

namespace FastAtomicLazy.Benchmarks.Configs
{
    /// <summary>
    /// Basic BenchmarkDotNet configuration used for microbenchmarks.
    /// </summary>
    public class MicroBenchmarkConfig : ManualConfig
    {
        public MicroBenchmarkConfig()
        {
            this.AddDiagnoser(MemoryDiagnoser.Default);
            this.AddExporter(MarkdownExporter.GitHub);
        }
    }
}