using System.Text.Json;
using System.Text.Json.Serialization;
using GlassView.Core.Models;

namespace GlassView.Core.Serialization;

// This class can be deleted once the following issue is resolved:
// --> https://github.com/dotnet/runtime/issues/63791
internal sealed class BenchmarkSerializer : JsonConverter<BenchmarkSummary>
{
    public override void Write(Utf8JsonWriter writer, BenchmarkSummary value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        WriteBuiltInTypes(new JsonWriter(writer), value);
        WriteGlassViewTypes(writer, value, options);
        writer.WriteEndObject();

        static void WriteBuiltInTypes(JsonWriter writer, BenchmarkSummary value)
        {
            writer.Write(nameof(value.Name), value.Name);
            writer.Write(nameof(value.Count), value.Count);
            writer.Write(nameof(value.Namespace), value.Namespace);
            writer.Write(nameof(value.Timestamp), value.Timestamp);
            writer.Write(nameof(value.Duration), value.Duration);
        }

        static void WriteGlassViewTypes(Utf8JsonWriter writer, BenchmarkSummary value, JsonSerializerOptions options)
        {
            writer.WritePropertyName(nameof(value.Environment));
            JsonSerializer.Serialize(writer, value.Environment, options);
            writer.WritePropertyName("BenchmarkCases");
            JsonSerializer.Serialize(writer, (IEnumerable<BenchmarkCase>)value, options);
        }
    }

    public override BenchmarkSummary? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) => throw new NotImplementedException();
}

// To be deleted, too. (see above)
// For now, it's just a handy way of consistently writing different types of values.
file sealed class JsonWriter(Utf8JsonWriter writer)
{
    public void Write(String name, Double value) => writer.WriteNumber(name, value);
    public void Write(String name, String value) => writer.WriteString(name, value);
    public void Write(String name, DateTime value) => writer.WriteString(name, value);
    public void Write(String name, TimeSpan value) => writer.WriteString(name, value.ToString("c"));
}
