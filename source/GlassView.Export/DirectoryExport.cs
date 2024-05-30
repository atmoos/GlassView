using System.Text.Json;
using BenchmarkDotNet.Loggers;
using Atmoos.GlassView.Core.Models;

namespace Atmoos.GlassView.Export;

internal sealed class DirectoryExport(DirectoryInfo path, JsonSerializerOptions options) : IExport
{
    public async Task Export(BenchmarkSummary summary, ILogger logger, CancellationToken token)
    {
        FileInfo file = path.AddFile(FileNameFor(summary));
        logger.WriteLineInfo($"file: {file.FullName}");
        using var stream = file.Open(FileMode.Create, FileAccess.Write, FileShare.None);
        await JsonSerializer.SerializeAsync(stream, summary, options, token).ConfigureAwait(ConfigureAwaitOptions.None);
    }

    public override String ToString() => $"{nameof(Export)}: {path.FullName}";

    private static String FileNameFor(BenchmarkSummary summary)
        => $"{summary.Name}-{summary.Timestamp.ToLocalTime():s}.json".Replace(':', '-');
}
