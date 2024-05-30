using System.Text.Json;
using BenchmarkDotNet.Loggers;
using Atmoos.GlassView.Core;
using Atmoos.GlassView.Core.Models;

namespace Atmoos.GlassView.Export.Test;

public class DirectoryExportTest
{
    private static readonly FileInfo testFile = new(Path.Combine("Resources", "TestBenchmark.json"));

    [Fact]
    public async Task ExportWritesJsonFile()
    {
        var options = new JsonSerializerOptions().EnableGlassView();
        var summary = testFile.Deserialize<BenchmarkSummary>(options);
        var emptyDirectory = Directory.CreateDirectory(Guid.NewGuid().ToString());
        var export = new DirectoryExport(emptyDirectory, options);

        try {

            await export.Export(summary, NullLogger.Instance, CancellationToken.None);

            var exportedFile = Assert.Single(emptyDirectory.GetFiles());
            Assert.StartsWith(summary.Name, exportedFile.Name);
            Assert.EndsWith(".json", exportedFile.Name);
        }
        finally {
            emptyDirectory.Delete(true);
        }
    }

    [Fact]
    public void ToStringContainsExportPath()
    {
        String[] pathComponents = ["path", "to", "directory"];
        var path = new DirectoryInfo(Path.Combine(pathComponents));
        var export = new DirectoryExport(path, JsonSerializerOptions.Default);

        var result = export.ToString();

        // Assert
        Assert.All(pathComponents, component => Assert.Contains(component, result));
    }
}
