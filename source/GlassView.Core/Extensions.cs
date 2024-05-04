using System.Text.Json;
using GlassView.Core.Serialization;

namespace GlassView.Core;

public static class Extensions
{
    public static JsonSerializerOptions EnableGlassView(this JsonSerializerOptions options)
    {
        options.Converters.Add(new BenchmarkSerializer());
        return options;
    }
}
