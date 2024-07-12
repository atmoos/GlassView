using System.Text;
using Atmoos.Sphere.Functional;
using Atmoos.World;
using BenchmarkDotNet.Loggers;
using Microsoft.Extensions.Configuration;

using TestFileSystem = Atmoos.World.InMemory.IO.UnixFileSystem<Atmoos.World.InMemory.Time>;

namespace Atmoos.GlassView.Export.Test;

public class GlassViewTest
{

    [Fact]
    public void ConfigureWithDirectoryReturnsDirectoryExporter()
    {
        String path = System.IO.Path.Combine("some", "export", "path");
        IConfiguration configuration = ConfigFromJson(DirectoryExportJson(path));
        IGlassView publicExporter = GlassView.Configure<TestFileSystem>(configuration, ConsoleLogger.Default);

        var exporter = Assert.IsType<SummaryExporter>(publicExporter);
        var directoryExporter = Assert.Single(exporter);
        Assert.IsType<DirectoryExport<TestFileSystem>>(directoryExporter);
    }

    [Fact]
    public void ConfigureWithDirectoryCreatesTheExportPathAsNeeded()
    {
        var pathSpec = System.IO.Path.Combine(RootName(), "this", "path", "does", "not", "exist", "yet");
        var path = Path.Parse<TestFileSystem>(pathSpec);
        IConfiguration configuration = ConfigFromJson(DirectoryExportJson(pathSpec));

        Assert.IsType<Failure<IDirectory>>(TestFileSystem.Search(path)); // Sanity check

        var exporter = GlassView.Configure<TestFileSystem>(configuration, ConsoleLogger.Default);
        Assert.IsType<Success<IDirectory>>(TestFileSystem.Search(path));
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
