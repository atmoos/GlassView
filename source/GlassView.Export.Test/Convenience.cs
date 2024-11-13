using System.IO;
using System.Text.Json;

namespace Atmoos.GlassView.Export.Test;

internal static class Convenience
{
    public static T Deserialize<T>(String text, JsonSerializerOptions options)
    {
        return JsonSerializer.Deserialize<T>(text, options) ?? throw new InvalidOperationException("Failed deserializing a string.");
    }

    public static T Deserialize<T>(this FileInfo file, JsonSerializerOptions options)
    {
        using var stream = file.OpenRead();
        return JsonSerializer.Deserialize<T>(stream, options) ?? throw new InvalidOperationException($"Failed deserializing file '{file.Name}'.");
    }
}

