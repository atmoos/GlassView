using System.Text.Json;
using Atmoos.World;
using Atmoos.World.IO.FileSystem;
using Atmoos.GlassView.Core;
using Microsoft.Extensions.Configuration;
using BenchmarkDotNet.Loggers;
using Atmoos.GlassView.Export.Configuration;

using static Atmoos.World.Extensions;
using static Atmoos.GlassView.Export.Extensions;
using static Atmoos.GlassView.Export.Configuration.Extensions;

using DirectoryExport = Atmoos.GlassView.Export.DirectoryExport<Atmoos.World.IO.FileSystem.Current>;
using Atmoos.Sphere.Functional;

namespace Atmoos.GlassView.Export;

public static class GlassView
{
    private const String glassViewSection = nameof(GlassView);
    private static readonly DirectoryName glassViewDirectory = new(glassViewSection);
    private static readonly DirectoryName artifactDirectory = new("BenchmarkDotNet.Artifacts");
    public static IGlassView Default() => Default(ConsoleLogger.Default);
    public static IGlassView Default(ILogger logger)
        => new SummaryExporter(new DirectoryExport(GlassViewArtifactsDirectory(), SerializationOptions()), logger);

    public static IGlassView Configure(IConfiguration config)
        => Configure(config, ConsoleLogger.Default);
    public static IGlassView Configure(IConfiguration config, ILogger logger)
    {
        logger.WriteLine(); // this helps visually separate us from BenchmarkDotNet's output.
        IConfigurationSection section = config.GetSection(glassViewSection);
        if (!section.Exists()) {
            logger.WriteHint($"No configuration section '{glassViewSection}' found. Using default directory exporter.");
            return Default(logger);
        }
        var exporters = new List<IExport>();
        var export = section.Section<Configuration.Export>();
        var serializationOptions = SerializationOptions().Configure(export.JsonFormatting);

        // Here's were we can add more exporters in the future.
        Set(export.Directory?.Path, path => exporters.Add(new DirectoryExport(ParseExportConfig(path), serializationOptions)));

        if (exporters.Count == 0) {
            logger.WriteHint($"No exporters configured. Using default directory exporter.");
            exporters.Add(new DirectoryExport(GlassViewArtifactsDirectory(), serializationOptions));
        }

        return new SummaryExporter(exporters, logger);
    }

    private static IDirectory GlassViewArtifactsDirectory() => FindLeaf<Current>(artifactDirectory) switch {
        Success<IDirectory> success => Current.Create(success.Value(), glassViewDirectory),
        _ => Current.Create(Path.Rel<Current>(artifactDirectory, glassViewDirectory))
    };
    private static JsonSerializerOptions SerializationOptions() => new JsonSerializerOptions().EnableGlassView();
    private static IDirectory ParseExportConfig(String path) => Current.Create(Path.Parse<Current>(path));
}
