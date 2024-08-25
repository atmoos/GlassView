using System.Text.Json;
using BenchmarkDotNet.Loggers;
using Atmoos.World;
using Atmoos.GlassView.Core;
using Atmoos.GlassView.Core.Models;
using Xunit.Abstractions;

namespace Atmoos.GlassView.Export.Test;

public class DirectoryExportTest(ITestOutputHelper output)
{
    private readonly ILogger logger = new TestLogger(output);
    private static readonly JsonSerializerOptions options = new JsonSerializerOptions().EnableGlassView();
    private static readonly System.IO.FileInfo testFile = new(System.IO.Path.Combine("Resources", "TestBenchmark.json"));

    [Fact]
    public async Task ExportWritesJsonFile()
    {
        var summary = testFile.Deserialize<BenchmarkSummary>(options);

        var exportedFile = await RunExportTest(summary);

        Assert.StartsWith(summary.Name, exportedFile.Name);
        Assert.Equal("json", exportedFile.Name.Extension);
    }

    [Fact]
    public async Task ExportWritesExpectedJsonFile()
    {
        var summary = testFile.Deserialize<BenchmarkSummary>(options);

        var exportedFile = await RunExportTest(summary);

        using var actualText = exportedFile.OpenText();
        using var expectedText = testFile.OpenText();
        var expected = expectedText.ReadToEnd().ReplaceLineEndings(String.Empty);
        Assert.Equal(expected, actualText.ReadToEnd(), ignoreAllWhiteSpace: true);
    }

    [Fact]
    public void ToStringContainsExportPath()
    {
        String[] pathComponents = ["path", "to", "directory"];
        var path = Path.Abs(TestFileSystem.Root, pathComponents);
        var directory = TestFileSystem.Create(path);
        var export = new DirectoryExport<TestFileSystem>(directory, JsonSerializerOptions.Default);

        var result = export.ToString();

        Assert.All(pathComponents, component => Assert.Contains(component, result));
    }

    private static IDirectory CreateUniqueDirectory()
    {
        var uniqueName = Guid.NewGuid().ToString();
        return TestFileSystem.Create(TestFileSystem.Root, new DirectoryName(uniqueName));
    }

    private async Task<IFile> RunExportTest(BenchmarkSummary summary)
    {
        var directory = CreateUniqueDirectory();
        var export = new DirectoryExport<TestFileSystem>(directory, options);
        await export.Export(summary, this.logger, CancellationToken.None).ConfigureAwait(false);
        return Assert.Single(directory);
    }
}
