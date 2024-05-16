using System.Text.Json;
using System.Text.Json.Serialization;
using Atmoos.GlassView.Core.Models;

using JR = System.Text.Json.Utf8JsonReader;

using static System.Text.Json.JsonTokenType;

namespace Atmoos.GlassView.Core.Serialization;

// ToDo #10: Remove when upstream issue is resolved.
// --> https://github.com/dotnet/runtime/issues/63791
internal sealed class BenchmarkSerializer : JsonConverter<BenchmarkSummary>
{
    private const String benchmarkCases = "BenchmarkCases";

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
            writer.WritePropertyName(benchmarkCases);
            JsonSerializer.Serialize(writer, (IEnumerable<BenchmarkCase>)value, options);
        }
    }

    public override BenchmarkSummary? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        Int32 readProperties = 0;
        const Int32 properties = 7;
        var builder = new SummaryBuilder();
        var depth = reader.CurrentDepth;

        while (readProperties++ < properties) {

            String propertyName = reader.ReadPropertyName();

            switch (propertyName) {
                case nameof(BenchmarkSummary.Name):
                    builder.Name = reader.ReadString();
                    break;
                case nameof(BenchmarkSummary.Count):
                    builder.Count = reader.ReadInt32();
                    break;
                case nameof(BenchmarkSummary.Namespace):
                    builder.Namespace = reader.ReadString();
                    break;
                case nameof(BenchmarkSummary.Timestamp):
                    builder.Timestamp = reader.ReadDateTime();
                    break;
                case nameof(BenchmarkSummary.Duration):
                    builder.Duration = reader.ReadTimeSpan();
                    break;
                case nameof(BenchmarkSummary.Environment):
                    builder.Environment = reader.Deserialize<EnvironmentInfo>(options);
                    break;
                case benchmarkCases:
                    builder.BenchmarkCases = reader.Deserialize<List<BenchmarkCase>>(options);
                    break;
                default:
                    throw new ArgumentException($"Unexpected property name: {propertyName}");
            }
        }

        reader.UnwindTo(depth);

        if (builder.BenchmarkCases.Count != builder.Count) {
            throw new ArgumentException($"Expected '{builder.Count}' benchmark cases, but found '{builder.BenchmarkCases.Count}' instead.");
        }

        return new BenchmarkSummary(builder.BenchmarkCases) {
            Name = builder.Name,
            Count = builder.Count,
            Namespace = builder.Namespace,
            Timestamp = builder.Timestamp,
            Duration = builder.Duration,
            Environment = builder.Environment,
        };
    }
}

// ToDo #10: Delete these convenience classes. (Same as above)
// For now, it's just a handy way of consistently writing different types of values.
file sealed class JsonWriter(Utf8JsonWriter writer)
{
    public void Write(String name, Int32 value) => writer.WriteNumber(name, value);
    public void Write(String name, String value) => writer.WriteString(name, value);
    public void Write(String name, DateTime value) => writer.WriteString(name, value);
    public void Write(String name, TimeSpan value) => writer.WriteString(name, value.ToString("c"));
}

file sealed class SummaryBuilder : IName
{
    public String Name { get; set; } = System.String.Empty;
    public Int32 Count { get; set; } = 0;
    public String Namespace { get; set; } = System.String.Empty;
    public DateTime Timestamp { get; set; } = DateTime.MinValue;
    public TimeSpan Duration { get; set; } = TimeSpan.Zero;
    public EnvironmentInfo Environment { get; set; } = default!;
    public List<BenchmarkCase> BenchmarkCases { get; set; } = [];
}

internal static class ReadConvenience
{
    private delegate T Value<T>(ref JR reader);
    public static JR MoveTo(this ref JR reader, JsonTokenType target)
    {
        while (reader.TokenType != target && reader.Read()) { }
        return reader;
    }
    public static String ReadPropertyName(this ref JR reader) => MoveTo(ref reader, PropertyName).GetString() ?? throw new ArgumentException("Failed reading a property name.");
    public static Int32 ReadInt32(this ref JR reader) => MoveTo(ref reader, Number).GetInt32();
    public static String ReadString(this ref JR reader) => MoveTo(ref reader, JsonTokenType.String).GetString() ?? throw new ArgumentException("Failed reading a string.");
    public static DateTime ReadDateTime(this ref JR reader) => MoveTo(ref reader, JsonTokenType.String).GetDateTime();
    public static TimeSpan ReadTimeSpan(this ref JR reader) => MoveTo(ref reader, JsonTokenType.String).GetString() is String str ? TimeSpan.Parse(str) : TimeSpan.Zero;
    public static void UnwindTo(this ref JR reader, Int32 depth)
    {
        while (reader.CurrentDepth > depth && reader.Read()) { }
    }
    public static T Deserialize<T>(this ref JR reader, JsonSerializerOptions options)
    {
        Int32 depth = reader.CurrentDepth;
        T value = JsonSerializer.Deserialize<T>(ref reader, options) ?? throw new ArgumentException("Deserialization failed.");
        reader.UnwindTo(depth);
        return value;
    }
}
