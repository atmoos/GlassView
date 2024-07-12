using System.Text.Json;
using BenchmarkDotNet.Loggers;
using Atmoos.GlassView.Core;
using Atmoos.GlassView.Core.Models;

using FileSystem = Atmoos.World.InMemory.IO.UnixFileSystem<Atmoos.World.InMemory.Time>;
using DirectoryExport = Atmoos.GlassView.Export.DirectoryExport<Atmoos.World.InMemory.IO.UnixFileSystem<Atmoos.World.InMemory.Time>>;
using Atmoos.World;

namespace Atmoos.GlassView.Export.Test;

public class DirectoryExportTest
{
    private static readonly System.IO.FileInfo testFile = new(System.IO.Path.Combine("Resources", "TestBenchmark.json"));

    [Fact]
    public async Task ExportWritesJsonFile()
    {
        var options = new JsonSerializerOptions().EnableGlassView();
        var summary = testFile.Deserialize<BenchmarkSummary>(options);
        var emptyDirectory = CreateUniqueDirectory();
        var export = new DirectoryExport(emptyDirectory, options);
        await export.Export(summary, NullLogger.Instance, CancellationToken.None);

        var exportedFile = Assert.Single(emptyDirectory);
        Assert.StartsWith(summary.Name, exportedFile.Name);
        Assert.EndsWith(".json", exportedFile.Name);
    }

    [Fact]
    public void ToStringContainsExportPath()
    {
        String[] pathComponents = ["path", "to", "directory"];
        var path = Path.Abs(FileSystem.Root, pathComponents);
        var directory = FileSystem.Create(path);
        var export = new DirectoryExport(directory, JsonSerializerOptions.Default);

        var result = export.ToString();

        // Assert
        Assert.All(pathComponents, component => Assert.Contains(component, result));
    }

    private static IDirectory CreateUniqueDirectory()
    {
        var uniqueName = Guid.NewGuid().ToString();
        return FileSystem.Create(FileSystem.Root, new DirectoryName(uniqueName));
    }
}
