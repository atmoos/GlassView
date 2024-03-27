using BenchmarkDotNet.Reports;
using GlassView.Export.Models;

namespace GlassView.Export;

public sealed class Exporter(DirectoryInfo path) : IExporter
{
    public async Task Export(Summary summary, CancellationToken token)
    {
        var values = Benchmark.Create(summary);
        var json = values.Serialize();
        var filePath = Path.Combine(path.FullName, $"{values.Name}.json");
        await File.WriteAllTextAsync(filePath, json, token).ConfigureAwait(false);
    }
}
