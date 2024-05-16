using System;
using System.Linq;
using BenchmarkDotNet.Attributes;

namespace Atmoos.GlassView.Benchmark;

[ShortRunJob]
[IterationCount(7)]
[MemoryDiagnoser]
public class OtherTestBenchmark
{
    private const Int32 count = 8096;

    [Benchmark(Baseline = true), BenchmarkCategory("SomeCat")]
    public void SumStrings() => Thread.Sleep(7);

    [Benchmark, BenchmarkCategory("OtherCat")]
    public void SumChars() => Thread.Sleep(9);
}
