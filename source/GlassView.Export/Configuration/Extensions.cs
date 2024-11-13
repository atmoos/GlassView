using System.Text.Json;
using Microsoft.Extensions.Configuration;

using static Atmoos.GlassView.Export.Extensions;

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
}
