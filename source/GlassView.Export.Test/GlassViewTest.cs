using Microsoft.Extensions.Configuration;
using System.Text;

namespace Atmoos.GlassView.Export.Test;

public class GlassViewTest
{

    [Fact]
    public void ConfigureWithDirectory_ReturnsDirectoryExporter()
    {
        String path = Path.Combine("some", "export", "path");
        IConfiguration configuration = ReadFromJson(DirectoryExportJson(path));
        IGlassView publicExporter = GlassView.Configure(configuration);

        var exporter = Assert.IsType<SummaryExporter>(publicExporter);
        var directoryExporter = Assert.Single(exporter);
        Assert.IsType<DirectoryExport>(directoryExporter);
    }

    private static IConfiguration ReadFromJson(String json)
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
