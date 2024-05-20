using Microsoft.Extensions.Configuration;
using System.Text;

namespace Atmoos.GlassView.Export.Test;

public class GlassViewTest
{

    [Fact]
    public void ConfigureWithDirectoryReturnsDirectoryExporter()
    {
        String path = Path.Combine("some", "export", "path");
        IConfiguration configuration = ConfigFromJson(DirectoryExportJson(path));
        IGlassView publicExporter = GlassView.Configure(configuration);

        var exporter = Assert.IsType<SummaryExporter>(publicExporter);
        var directoryExporter = Assert.Single(exporter);
        Assert.IsType<DirectoryExport>(directoryExporter);
    }

    [Fact]
    public void ConfigureWithDirectoryCreatesTheExportPathAsNeeded()
    {
        const String topPath = "top";
        var path = Path.Combine(topPath, "this", "path", "does", "not", "exist", "yet");
        IConfiguration configuration = ConfigFromJson(DirectoryExportJson(path));

        Assert.False(Directory.Exists(path), "Precondition failed: directory already exists.");

        try {
            var exporter = GlassView.Configure(configuration);
            Assert.True(Directory.Exists(path), "The directory was not created.");
            Assert.NotNull(exporter); // Avoid warnings & the compiler optimising away the configure step.
        }
        finally {
            Directory.Delete(topPath, recursive: true);
        }
    }

    private static IConfiguration ConfigFromJson(String json)
    {
        return new ConfigurationBuilder()
            .AddJsonStream(new MemoryStream(Encoding.UTF8.GetBytes(json)))
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
}
