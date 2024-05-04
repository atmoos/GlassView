using System.Text.Json;
using GlassView.Core.Models;

namespace GlassView.Export.Serialization;

// This class can be deleted once the following issue is resolved:
// --> https://github.com/dotnet/runtime/issues/63791
internal sealed class BenchmarkSerializer : System.Text.Json.Serialization.JsonConverter<BenchmarkSummary>
{
    public override void Write(Utf8JsonWriter writer, BenchmarkSummary value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        WriteBenchmark(new JsonWriter(writer), value);
        writer.WritePropertyName(nameof(value.Environment));
        JsonSerializer.Serialize(writer, value.Environment, options);
        writer.WritePropertyName("BenchmarkCases");
        JsonSerializer.Serialize(writer, (IEnumerable<BenchmarkCase>)value, options);
        writer.WriteEndObject();

        static void WriteBenchmark(JsonWriter writer, BenchmarkSummary value)
        {
            writer.Write(nameof(value.Name), value.Name);
            writer.Write(nameof(value.Count), value.Count);
            writer.Write(nameof(value.Namespace), value.Namespace);
            writer.Write(nameof(value.Timestamp), value.Timestamp);
            writer.Write(nameof(value.Duration), value.Duration);
        }
    }
    public override BenchmarkSummary? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) => throw new NotImplementedException();
}

file sealed class JsonWriter
{
    private readonly Utf8JsonWriter writer;
    public JsonWriter(in Utf8JsonWriter writer) => this.writer = writer;
    public void Write(String name, Double value) => this.writer.WriteNumber(name, value);
    public void Write(String name, String value) => this.writer.WriteString(name, value);
    public void Write(String name, DateTime value) => this.writer.WriteString(name, value);
    public void Write(String name, TimeSpan value) => this.writer.WriteString(name, value.ToString("c"));
}
