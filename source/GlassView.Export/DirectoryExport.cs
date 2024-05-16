using System.Text.Json;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Reports;
using Atmoos.GlassView.Core;
using Atmoos.GlassView.Core.Models;
using Atmoos.GlassView.Export.Configuration;

using static Atmoos.GlassView.Export.Mapping;
using static Atmoos.GlassView.Export.Configuration.Extensions;

namespace Atmoos.GlassView.Export;

internal sealed class DirectoryExport(DirectoryInfo path, ILogger logger) : IExport
{
    private static readonly JsonSerializerOptions defaultOptions = new JsonSerializerOptions().EnableGlassView();
    private readonly JsonSerializerOptions options = defaultOptions;

    public DirectoryExport(DirectoryInfo path, JsonFormatting formatting, ILogger logger)
        : this(path, logger)
    {
        this.options = new JsonSerializerOptions().EnableGlassView();
        SetValue(formatting.Indented, value => this.options.WriteIndented = value);
        SetValue(formatting.AllowTrailingCommas, value => this.options.AllowTrailingCommas = value);
    }

    public async Task Export(Summary inputSummary, CancellationToken token)
    {
        var summary = Map(inputSummary);
        FileInfo file = path.AddFile(FileNameFor(summary));
        logger.WriteLine($"Exporting summary '{summary.Name}' to:");
        logger.WriteLine($" -> {file.FullName}");
        using var stream = file.Open(FileMode.Create, FileAccess.Write, FileShare.None);
        await JsonSerializer.SerializeAsync(stream, summary, this.options, token).ConfigureAwait(ConfigureAwaitOptions.None);
    }
    public override String ToString() => $"{nameof(Export)}: {path.FullName}";

    private static String FileNameFor(BenchmarkSummary summary) => $"{summary.Name}-{summary.Timestamp.ToLocalTime():s}.json";
}
