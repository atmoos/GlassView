using System.Text.Json;
using Microsoft.Extensions.Configuration;

namespace Atmoos.GlassView.Export.Configuration;

internal static class Extensions
{
    public static JsonSerializerOptions Configure(this JsonSerializerOptions options, JsonFormatting? formatting)
    {
        if (formatting is null) {
            return options;
        }
        SetValue(formatting.Indented, value => options.WriteIndented = value);
        SetValue(formatting.AllowTrailingCommas, value => options.AllowTrailingCommas = value);
        return options;
    }

    /// <summary>
    /// Safely get a configuration section as a strongly-typed object.
    /// </summary>
    public static T Section<T>(this IConfiguration config) => config.GetSection(typeof(T).Name).Get<T>();

    private static void SetValue<T>(T? value, Action<T> setter)
        where T : struct
    {
        if (value != null) {
            setter(value.Value);
        }
    }

    private static void Set<T>(T? value, Action<T> setter)
    where T : class
    {
        if (value != null) {
            setter(value);
        }
    }

}
