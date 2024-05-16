using System;
using System.Linq;
using BenchmarkDotNet.Attributes;

namespace Atmoos.GlassView.Benchmark;

[ShortRunJob]
[IterationCount(7)]
[MemoryDiagnoser]
public class TestBenchmark
{
    [Params(54, 42)]
    public Int32 Count { get; set; }

    [Benchmark(Baseline = true), BenchmarkCategory("SomeCat")]
    public Int32 SumIntegers() => Integers(Count).Sum();

    [Benchmark, BenchmarkCategory("OtherCat")]
    public Double SumDoubles() => Doubles(Count).Sum();
    private static Double[] Doubles(Int32 count) => Enumerable.Range(0, count).Select(i => i - count / 2d).ToArray();
    private static Int32[] Integers(Int32 count) => Enumerable.Range(0, count).Select(i => i - count / 2).ToArray();
}
