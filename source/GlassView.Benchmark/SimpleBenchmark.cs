using BenchmarkDotNet.Attributes;

namespace Atmoos.GlassView.Benchmark;

/* This is only a dummy benchmark for us to play around with exports. */

[ShortRunJob]
[IterationCount(7)]
// So simple, that I don't even add a category...
public class SimpleBenchmark
{
    private static readonly String[] strings = Enumerable.Range(0, 321).Select(i => i.ToString()).ToArray();

    [Benchmark]
    public String ConcatStrings()
        => HorribleStringConcatenationNeverEverUseThis(strings);

    private static String HorribleStringConcatenationNeverEverUseThis(IEnumerable<String> strings)
    {
        return strings.Aggregate(String.Empty, (a, b) => a + b);
    }
}
