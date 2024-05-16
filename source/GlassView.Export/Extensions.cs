using System.Text.Json;
using BenchmarkDotNet.Reports;
using Atmoos.GlassView.Core;
using Microsoft.Extensions.Configuration;
using static System.Threading.Tasks.ConfigureAwaitOptions;

namespace Atmoos.GlassView.Export;

public static class Extensions
{
    public static async Task Export(this IExport exporter, IEnumerable<Summary> inputSummaries, CancellationToken token = default)
    {
        // For fun, some interleaved exporting :-)
        Task export = Task.CompletedTask;
        foreach (Summary summary in inputSummaries) {
            Task next = exporter.Export(summary, token);
            await export.ConfigureAwait(None);
            export = next;
        }
        await export.ConfigureAwait(None);
    }

    internal static String Serialize<T>(this T value) => Serialize(value, Options(new JsonSerializerOptions()));
    internal static String Serialize<T>(this T value, JsonSerializerOptions options) => JsonSerializer.Serialize(value, options);

    private static JsonSerializerOptions Options(JsonSerializerOptions options)
    {
        options.WriteIndented = true;
        return options.EnableGlassView();
    }

    internal static T Item<T>(this IConfiguration config) => config.GetSection(typeof(T).Name).Get<T>();
}
