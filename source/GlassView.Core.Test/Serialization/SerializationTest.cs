using Atmoos.GlassView.Core.Models;

namespace Atmoos.GlassView.Core.Test;

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
        Assert.Equal(actual.Count, actual.Count());
    }

    [Fact]
    public void RoundRobinSerializationSucceeds()
    {
        BenchmarkSummary expected = File.ReadAllText(testFile).Deserialize<BenchmarkSummary>();

        String serializedSummary = expected.Serialize();
        BenchmarkSummary actual = serializedSummary.Deserialize<BenchmarkSummary>();

        AssertEqual(expected, actual);
    }

    // ToDo: For some reason the standard equality check fails, so asserting each property individually for now...
    private static void AssertEqual(BenchmarkSummary expected, BenchmarkSummary actual)
    {
        Assert.Equal(expected.Name, actual.Name);
        Assert.Equal(expected.Count, actual.Count);
        Assert.Equal(expected.Namespace, actual.Namespace);
        Assert.Equal(expected.Timestamp, actual.Timestamp);
        Assert.Equal(expected.Duration, actual.Duration);
        Assert.Equal(expected.Environment, actual.Environment);

        foreach (var (expectedCase, actualCase) in expected.Zip(actual)) {
            Equal(expectedCase, actualCase);
        }

        static void Equal(BenchmarkCase expected, BenchmarkCase actual)
        {
            Assert.Equal(expected.Name, actual.Name);
            Assert.Equal(expected.IsBaseline, actual.IsBaseline);
            Assert.Equal(expected.Categories, actual.Categories);
            Assert.Equal(expected.Statistics, actual.Statistics);
            Assert.Equal(expected.Allocation, actual.Allocation);
        }
    }
}
