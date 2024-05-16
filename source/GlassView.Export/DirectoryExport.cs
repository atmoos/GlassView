using System.Text.Json;
using BenchmarkDotNet.Reports;
using Atmoos.GlassView.Core;
using Atmoos.GlassView.Core.Models;
using Atmoos.GlassView.Export.Configuration;

using static Atmoos.GlassView.Export.Mapping;
using static Atmoos.GlassView.Export.Extensions;

namespace Atmoos.GlassView.Export;

internal sealed class DirectoryExport(DirectoryInfo path) : IExport
{
    private static readonly JsonSerializerOptions defaultOptions = new JsonSerializerOptions().EnableGlassView();

    private readonly JsonSerializerOptions options = defaultOptions;

    public DirectoryExport(DirectoryInfo path, JsonFormatting formatting)
        : this(path)
    {
        this.options = new JsonSerializerOptions().EnableGlassView();
        Set(formatting?.Indented, value => this.options.WriteIndented = value);
        Set(formatting?.AllowTrailingCommas, value => this.options.AllowTrailingCommas = value);
    }

    public async Task Export(Summary inputSummary, CancellationToken token)
    {
        var summary = Map(inputSummary);
        FileInfo file = CombineToFile(path, summary);
        using var stream = file.Open(FileMode.Create, FileAccess.Write, FileShare.None);
        await JsonSerializer.SerializeAsync(stream, summary, this.options, token).ConfigureAwait(ConfigureAwaitOptions.None);
    }
    public override String ToString() => $"{nameof(Export)}: {path.FullName}";

    private static FileInfo CombineToFile(DirectoryInfo path, BenchmarkSummary summary)
        => new(Path.Combine(path.FullName, $"{summary.Name}.json"));
}
