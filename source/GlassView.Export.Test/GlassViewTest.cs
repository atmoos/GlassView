using System.Text;
using Atmoos.Sphere.Functional;
using Atmoos.World;
using BenchmarkDotNet.Loggers;
using Microsoft.Extensions.Configuration;
using Xunit.Abstractions;

namespace Atmoos.GlassView.Export.Test;

public class GlassViewTest(ITestOutputHelper output)
{
    private readonly ILogger logger = new TestLogger(output);

    [Fact]
    public void ConfigureWithDirectoryReturnsDirectoryExporter()
    {
        String path = System.IO.Path.Combine("some", "export", "path");
        IConfiguration configuration = ConfigFromJson(DirectoryExportJson(path));
        IGlassView publicExporter = GlassView.Configure<TestFileSystem>(configuration, this.logger);

        var exporter = Assert.IsType<SummaryExporter>(publicExporter);
        var directoryExporter = Assert.Single(exporter);
        Assert.IsType<DirectoryExport<TestFileSystem>>(directoryExporter);
    }

    [Fact]
    public void ConfigureWithDirectoryCreatesTheExportPathAsNeeded()
    {
        var exportDirName = "yet";
        var pathSpec = System.IO.Path.Combine(RootName(), "this", "path", "does", "not", "exist", exportDirName);
        var path = Path.Parse<TestFileSystem>(pathSpec);
        IConfiguration configuration = ConfigFromJson(DirectoryExportJson(pathSpec));

        Assert.IsType<Failure<IDirectory>>(TestFileSystem.Search(path)); // Sanity check

        var exporter = GlassView.Configure<TestFileSystem>(configuration, this.logger);
        var exportDir = Assert.IsType<Success<IDirectory>>(TestFileSystem.Search(path)).Value();
        Assert.True(exportDir.Exists);
        Assert.Equal(exportDirName, exportDir.Name);
        Assert.NotNull(exporter); // Avoid warnings & the compiler optimising away the configure step.
    }

    private static IConfiguration ConfigFromJson(String json)
    {
        return new ConfigurationBuilder()
            .AddJsonStream(new System.IO.MemoryStream(Encoding.UTF8.GetBytes(json)))
            .Build();
    }

    private static String DirectoryExportJson(String path)
    {
        return $$"""
                {
                    "GlassView": {
                        "Export": {
                            "Directory": {
                                "Path": "{{Sanitise(path)}}"
                             }
                        }
                    }
                }
                """;

        static String Sanitise(String value) => value.Replace("\\", "/"); // Windows path to Unix path, which are compatible with JSON.
    }

    private static String RootName()
    {
        return System.IO.Path.GetPathRoot(System.IO.Path.GetTempPath()) ?? throw new InvalidOperationException("No root path found.");
    }
}
