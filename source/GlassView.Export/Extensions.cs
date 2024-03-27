using System.Text.Json;
using BenchmarkDotNet.Reports;

using static System.Threading.Tasks.ConfigureAwaitOptions;

namespace GlassView.Export;


public static class Extensions
{
    public static async Task Export(this IExporter exporter, IEnumerable<Summary> summaries, CancellationToken token = default)
    {
        // For fun, some interleaved exporting :-)
        IEnumerator<Summary> enumerator = summaries.GetEnumerator();
        if (!enumerator.MoveNext()) {
            return;
        }
        Task export = exporter.Export(enumerator.Current, token);
        while (enumerator.MoveNext()) {
            Task next = exporter.Export(enumerator.Current, token);
            await export.ConfigureAwait(None);
            export = next;
        }
        await export.ConfigureAwait(None);
    }
    public static String Serialize<T>(this T value) => Serialize(value, Options());
    public static String Serialize<T>(this T value, JsonSerializerOptions options) => JsonSerializer.Serialize(value, options);

    private static JsonSerializerOptions Options() => new() { WriteIndented = true };
}
