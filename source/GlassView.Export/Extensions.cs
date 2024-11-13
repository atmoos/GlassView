using System.Text.Json;
using Atmoos.GlassView.Core;

namespace Atmoos.GlassView.Export;

public static class Extensions
{
    internal static String Serialize<T>(this T value) => Serialize(value, Options(new JsonSerializerOptions()));
    internal static String Serialize<T>(this T value, JsonSerializerOptions options) => JsonSerializer.Serialize(value, options);

    private static JsonSerializerOptions Options(JsonSerializerOptions options)
    {
        options.WriteIndented = true;
        return options.EnableGlassView();
    }

    internal static void SetValue<T>(T? value, Action<T> setter)
        where T : struct
    {
        if (value != null) {
            setter(value.Value);
        }
    }

    internal static void Set<T>(T? value, Action<T> setter)
        where T : class
    {
        if (value != null) {
            setter(value);
        }
    }
}
