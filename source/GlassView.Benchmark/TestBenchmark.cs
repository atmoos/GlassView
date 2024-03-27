using System;
using System.Linq;
using BenchmarkDotNet.Attributes;

namespace GlassView.Benchmark;

[ShortRunJob]
[IterationCount(7)]
[MemoryDiagnoser]
public class TestBenchmark
{
    private const Int32 count = 8096;

    [Benchmark(Baseline = true), BenchmarkCategory("SomeCat")]
    public Int32 SumIntegers() => Integers(count).Sum();

    [Benchmark, BenchmarkCategory("OtherCat")]
    public Double SumDoubles() => Doubles(count).Sum();
    private static Double[] Doubles(Int32 count) => Enumerable.Range(0, count).Select(i => i - count / 2d).ToArray();
    private static Int32[] Integers(Int32 count) => Enumerable.Range(0, count).Select(i => i - count / 2).ToArray();
}
