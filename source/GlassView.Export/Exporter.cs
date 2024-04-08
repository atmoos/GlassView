using BenchmarkDotNet.Reports;

namespace GlassView.Export;

public sealed class Exporter(DirectoryInfo path) : IExporter
{
    public async Task Export(Summary summary, CancellationToken token)
    {
        var values = GlassView.Export.Export.Map(summary);
        var json = values.Serialize();
        var filePath = Path.Combine(path.FullName, $"{values.Name}.json");
        await File.WriteAllTextAsync(filePath, json, token).ConfigureAwait(false);
    }
}
