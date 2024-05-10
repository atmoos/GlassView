using BenchmarkDotNet.Reports;
using GlassView.Core.Models;

using static GlassView.Export.Mapping;

namespace GlassView.Export;

internal sealed class DirectoryExport(DirectoryInfo path) : IExport
{
    public async Task Export(Summary summary, CancellationToken token)
    {
        BenchmarkSummary values = Map(summary);
        String json = values.Serialize();
        String filePath = Path.Combine(path.FullName, $"{values.Name}.json");
        await File.WriteAllTextAsync(filePath, json, token).ConfigureAwait(false);
    }
    public override String ToString() => $"{nameof(Export)}: {path.FullName}";
}
