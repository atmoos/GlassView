using System.Text.Json;
using BenchmarkDotNet.Reports;
using Atmoos.GlassView.Core;
using Atmoos.GlassView.Core.Models;

using static Atmoos.GlassView.Export.Mapping;

namespace Atmoos.GlassView.Export;

internal sealed class DirectoryExport(DirectoryInfo path) : IExport
{
    private static readonly JsonSerializerOptions options = new JsonSerializerOptions().EnableGlassView();
    public async Task Export(Summary inputSummary, CancellationToken token)
    {
        var summary = Map(inputSummary);
        FileInfo file = CombineToFile(path, summary);
        using var stream = file.Open(FileMode.Create, FileAccess.Write, FileShare.None);
        await JsonSerializer.SerializeAsync(stream, summary, options, token).ConfigureAwait(ConfigureAwaitOptions.None);
    }
    public override String ToString() => $"{nameof(Export)}: {path.FullName}";

    private static FileInfo CombineToFile(DirectoryInfo path, BenchmarkSummary summary)
        => new(Path.Combine(path.FullName, $"{summary.Name}.json"));
}
