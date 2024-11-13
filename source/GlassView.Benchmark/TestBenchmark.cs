using BenchmarkDotNet.Attributes;

namespace Atmoos.GlassView.Benchmark;

/* This is only a dummy benchmark for us to play around with exports. */

[ShortRunJob]
[IterationCount(7)]
[MemoryDiagnoser]
[BenchmarkCategory("Regular")]
public class TestBenchmark
{
    [Params(54, 42)]
    public Int32 Count { get; set; }

    [Benchmark(Baseline = true), BenchmarkCategory("SomeCategory")]
    public Int32 SumIntegers() => Integers(Count).Sum();

    [Benchmark, BenchmarkCategory("OtherCategory")]
    public Double SumDoubles() => Doubles(Count).Sum();
    private static Double[] Doubles(Int32 count) => Enumerable.Range(0, count).Select(i => i - count / 2d).ToArray();
    private static Int32[] Integers(Int32 count) => Enumerable.Range(0, count).Select(i => i - count / 2).ToArray();
}
