using System.Text.Json;
using System.Text.Json.Serialization;
using Atmoos.GlassView.Core.Models;

namespace Atmoos.GlassView.Core.Serialization;

// ToDo #10: Remove when upstream issue is resolved.
// --> https://github.com/dotnet/runtime/issues/63791
internal sealed class BenchmarkSerializer : JsonConverter<BenchmarkSummary>
{
    public override void Write(Utf8JsonWriter writer, BenchmarkSummary value, JsonSerializerOptions options)
    {
        var summary = new MutableBenchmarkSummary {
            Name = value.Name,
            Count = value.Count,
            Namespace = value.Namespace,
            Timestamp = value.Timestamp,
            Duration = value.Duration,
            Environment = value.Environment,
            BenchmarkCases = [.. value],
        };
        JsonSerializer.Serialize(writer, summary, options);
    }

    public override BenchmarkSummary? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var summary = JsonSerializer.Deserialize<MutableBenchmarkSummary>(ref reader, options) ?? throw new ArgumentException("Deserialization failed.");
        if (summary.Count != summary.BenchmarkCases.Count) {
            throw new ArgumentException($"Expected '{summary.Count}' benchmark cases, but found '{summary.BenchmarkCases.Count}' instead.");
        }

        return new BenchmarkSummary(summary.BenchmarkCases) {
            Name = summary.Name,
            Count = summary.Count,
            Namespace = summary.Namespace,
            Timestamp = summary.Timestamp,
            Duration = summary.Duration,
            Environment = summary.Environment,
        };
    }
}

// ToDo #10: Delete this convenience class. (Same as above)
file sealed class MutableBenchmarkSummary : IName
{
    public String Name { get; set; } = String.Empty;
    public Int32 Count { get; set; } = 0;
    public String Namespace { get; set; } = String.Empty;
    public DateTime Timestamp { get; set; } = DateTime.MinValue;
    public TimeSpan Duration { get; set; } = TimeSpan.Zero;
    public EnvironmentInfo Environment { get; set; } = default!;
    public List<BenchmarkCase> BenchmarkCases { get; set; } = [];
}
