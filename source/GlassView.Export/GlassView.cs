using BenchmarkDotNet.Reports;
using GlassView.Core.Models;

using static GlassView.Export.Mapping;

namespace GlassView.Export;

// ToDo: Parameterize the path in AppSettings.json
public sealed class GlassView(DirectoryInfo path) : IGlassView
{
    public async Task Export(Summary summary, CancellationToken token)
    {
        BenchmarkSummary values = Map(summary);
        String json = values.Serialize();
        String filePath = Path.Combine(path.FullName, $"{values.Name}.json");
        await File.WriteAllTextAsync(filePath, json, token).ConfigureAwait(false);
    }
}
