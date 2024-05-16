
using Microsoft.Extensions.Configuration;

namespace Atmoos.GlassView.Export.Configuration;

internal static class Extensions
{
    /// <summary>
    /// Safely get a configuration section as a strongly-typed object.
    /// </summary>
    internal static T Section<T>(this IConfiguration config) => config.GetSection(typeof(T).Name).Get<T>();

    /// <summary>
    /// Sets a nullable struct only when it's not null.
    /// </summary>
    internal static void SetValue<T>(T? value, Action<T> setter)
        where T : struct
    {
        if (value != null) {
            setter(value.Value);
        }
    }

    /// <summary>
    /// Sets a nullable class only when it's not null.
    /// </summary>
    internal static void Set<T>(T? value, Action<T> setter)
    where T : class
    {
        if (value != null) {
            setter(value);
        }
    }

}
