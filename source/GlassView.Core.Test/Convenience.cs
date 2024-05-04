using System.Text.Json;

namespace GlassView.Core.Test;

public static class Convenience
{
    private static readonly JsonSerializerOptions options = new JsonSerializerOptions() { WriteIndented = true }.EnableGlassView();

    public static String Serialize<T>(this T value) => JsonSerializer.Serialize(value, options);
    public static T Deserialize<T>(this String json) => JsonSerializer.Deserialize<T>(json, options) ?? throw new ArgumentException("Deserialization failed.", nameof(json));
}
