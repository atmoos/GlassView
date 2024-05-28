using System.Text.Json;

namespace Atmoos.GlassView.Core.Test;

public static class Convenience
{
    private static readonly JsonSerializerOptions options = new JsonSerializerOptions() { WriteIndented = true }.EnableGlassView();

    public static String Serialize<T>(this T value) => JsonSerializer.Serialize(value, options);
    public static T Deserialize<T>(this String json) => JsonSerializer.Deserialize<T>(json, options) ?? throw new ArgumentException("Deserialization failed.", nameof(json));
    public static T Deserialize<T>(this FileInfo json) => json.Deserialize<T>(options);
    public static T Deserialize<T>(this FileInfo json, JsonSerializerOptions options)
    {
        using var stream = json.OpenRead();
        return JsonSerializer.Deserialize<T>(stream, options) ?? throw new ArgumentException("Deserialization failed.", nameof(json));
    }
}
