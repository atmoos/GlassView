using GlassView.Core.Models;

namespace GlassView.Core.Test;

public class SerializationTest
{
    private static readonly String testFile = Path.Combine("Resources", "TestBenchmark.json");

    [Fact]
    public void SerializationIsPossible()
    {
        String expected = File.ReadAllText(testFile);
        BenchmarkSummary benchmarks = expected.Deserialize<BenchmarkSummary>();

        String actual = benchmarks.Serialize();

        Assert.Equal(expected, actual, ignoreAllWhiteSpace: true);
    }

    [Fact]
    public void DeserializationIsPossible()
    {
        BenchmarkSummary actual = File.ReadAllText(testFile).Deserialize<BenchmarkSummary>();

        Assert.NotNull(actual);
    }

    [Fact]
    public void RoundRobinSerializationSucceeds()
    {
        BenchmarkSummary expected = File.ReadAllText(testFile).Deserialize<BenchmarkSummary>();

        String serializedSummary = expected.Serialize();
        BenchmarkSummary actual = serializedSummary.Deserialize<BenchmarkSummary>();

        Assert.Equal(expected, actual);
    }
}
