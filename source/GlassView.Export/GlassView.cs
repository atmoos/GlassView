using System.Text.Json;
using Atmoos.World;
using Atmoos.World.IO.FileSystem;
using Atmoos.GlassView.Core;
using Atmoos.Sphere.Functional;
using Atmoos.GlassView.Export.Configuration;
using Microsoft.Extensions.Configuration;
using BenchmarkDotNet.Loggers;

using static Atmoos.World.Extensions;
using static Atmoos.GlassView.Export.Extensions;
using static Atmoos.GlassView.Export.Configuration.Extensions;

namespace Atmoos.GlassView.Export;

public static class GlassView
{
    private const String glassViewSection = nameof(GlassView);
    private static readonly DirectoryName glassViewDirectory = new(glassViewSection);
    private static readonly DirectoryName artifactDirectory = new("BenchmarkDotNet.Artifacts");
    public static IGlassView Default() => Default(ConsoleLogger.Default);
    public static IGlassView Default(ILogger logger) => Default<Current>(logger);
    internal static IGlassView Default<FileSystem>(ILogger logger)
        where FileSystem : IFileSystemState, IDirectoryCreation, IFileCreation
        => new SummaryExporter(new DirectoryExport<FileSystem>(GlassViewArtifactsDirectory<FileSystem>(), SerializationOptions()), logger);

    public static IGlassView Configure(IConfiguration config)
        => Configure(config, ConsoleLogger.Default);
    public static IGlassView Configure(IConfiguration config, ILogger logger)
        => Configure<Current>(config, logger);
    internal static IGlassView Configure<FileSystem>(IConfiguration config, ILogger logger)
        where FileSystem : IFileSystemState, IDirectoryCreation, IFileCreation
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
        Set(export.Directory?.Path, path => exporters.Add(new DirectoryExport<FileSystem>(ParseExportConfig<FileSystem>(path), serializationOptions)));

        if (exporters.Count == 0) {
            logger.WriteHint($"No exporters configured. Using default directory exporter.");
            exporters.Add(new DirectoryExport<FileSystem>(GlassViewArtifactsDirectory<FileSystem>(), serializationOptions));
        }

        return new SummaryExporter(exporters, logger);
    }

    private static IDirectory GlassViewArtifactsDirectory<FileSystem>()
        where FileSystem : IFileSystemState, IDirectoryCreation => FindLeaf<FileSystem>(artifactDirectory) switch {
            Success<IDirectory> success => FileSystem.Create(success.Value(), glassViewDirectory),
            _ => FileSystem.Create(Path.Rel<FileSystem>(artifactDirectory, glassViewDirectory))
        };
    private static JsonSerializerOptions SerializationOptions() => new JsonSerializerOptions().EnableGlassView();
    private static IDirectory ParseExportConfig<FileSystem>(String path)
        where FileSystem : IFileSystemState, IDirectoryCreation => FileSystem.Create(Path.Parse<FileSystem>(path));
}
