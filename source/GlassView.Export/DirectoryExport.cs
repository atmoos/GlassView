using System.Text.Json;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Reports;
using Atmoos.GlassView.Core.Models;

using static Atmoos.GlassView.Export.Mapping;

namespace Atmoos.GlassView.Export;

internal sealed class DirectoryExport(DirectoryInfo path, JsonSerializerOptions options, ILogger logger) : IExport
{
    public async Task Export(Summary inputSummary, CancellationToken token)
    {
        var summary = Map(inputSummary);
        FileInfo file = path.AddFile(FileNameFor(summary));
        logger.WriteLine($"Exporting summary '{summary.Name}' to:");
        logger.WriteLine($" -> {file.FullName}");
        using var stream = file.Open(FileMode.Create, FileAccess.Write, FileShare.None);
        await JsonSerializer.SerializeAsync(stream, summary, options, token).ConfigureAwait(ConfigureAwaitOptions.None);
    }
    public override String ToString() => $"{nameof(Export)}: {path.FullName}";

    private static String FileNameFor(BenchmarkSummary summary) => $"{summary.Name}-{summary.Timestamp.ToLocalTime():s}.json";
}
