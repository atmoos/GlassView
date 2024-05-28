using System.Text.Json;
using Atmoos.GlassView.Core.Models;

namespace Atmoos.GlassView.Core.Test;

public class SerializationTest
{
    private static readonly FileInfo testFile = new(Path.Combine("Resources", "TestBenchmark.json"));

    [Fact]
    // When issue #10 is resolved, we can remove the call to EnableGlassView().
    public void DeserializationIsPossibleAndDependsOnEnablingGlassView()
    {
        var options = new JsonSerializerOptions().EnableGlassView(); // required!
        BenchmarkSummary actual = testFile.Deserialize<BenchmarkSummary>(options);

        Assert.NotNull(actual);

        // The number of cases in the summary could differ if the json was tampered with (not relevant here)
        // or deserialization doesn't pick up the case child nodes (which is of relevance here).
#pragma warning disable CA1829 // Use Length/Count property instead of Count() when available
        Assert.Equal(actual.Count, actual.Count());
#pragma warning restore CA1829 // Use Length/Count property instead of Count() when available
    }

    /// <summary>
    /// Values that are sensitive to localisation may change during serialization.
    /// This test is a sanity check to ensure that serialization does not have any side effects
    /// on the values of the BenchmarkSummary.
    /// </summary>
    /// <remarks>This far from exhaustive or complete. Making that happen would be fairly involved and
    /// is not really relevant for now.
    /// e.g: This test was sufficient to pick up the sensitivity to localisation of the time stamp property,
    /// which we have since fixed to being UTC.</remarks>
    [Fact]
    public void RoundRobinSerializationDoesNotChangeAnyValues()
    {
        String expected = File.ReadAllText(testFile.FullName);

        // First step: Deserialize
        BenchmarkSummary benchmarks = expected.Deserialize<BenchmarkSummary>();

        // Second step: Serialize
        String actual = benchmarks.Serialize();

        // Round robin comparison is on strings, where we are only interested in the content (values).
        Assert.Equal(expected, actual, ignoreAllWhiteSpace: true, ignoreLineEndingDifferences: true);
    }

    /// <summary>
    /// Similar reasoning to <see cref="RoundRobinSerializationDoesNotChangeAnyValues"/>.
    /// This time it's just the other way around. (within reason).
    /// </summary>
    [Fact]
    public void RoundRobinDeserializationDoesNotChangeAnyValues()
    {
        BenchmarkSummary expected = testFile.Deserialize<BenchmarkSummary>();

        // First step: Serialize
        String serializedSummary = expected.Serialize();

        // Second step: Deserialize
        BenchmarkSummary actual = serializedSummary.Deserialize<BenchmarkSummary>();

        // Round robin comparison is on the summary object this time :-)
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
