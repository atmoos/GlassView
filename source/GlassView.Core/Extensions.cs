using System.Text.Json;
using Atmoos.GlassView.Core.Serialization;

namespace Atmoos.GlassView.Core;

public static class Extensions
{
    public static JsonSerializerOptions EnableGlassView(this JsonSerializerOptions options)
    {
        options.IgnoreReadOnlyProperties = true;
        options.Converters.Add(new BenchmarkSerializer());
        return options;
    }
}
