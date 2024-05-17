using System.Text.Json;
using Atmoos.GlassView.Core;
using Microsoft.Extensions.Configuration;
using BenchmarkDotNet.Loggers;
using Atmoos.GlassView.Export.Configuration;

using static System.IO.Directory;
using static Atmoos.GlassView.Export.Configuration.Extensions;

namespace Atmoos.GlassView.Export;

public static class GlassView
{
    private const String glassViewSection = nameof(GlassView);
    public static IExport Default() => Default(ConsoleLogger.Default);
    public static IExport Default(ILogger logger)
        => new DirectoryExport(GlassViewArtifactsDirectory(), SerializationOptions(), logger);

    public static IExport Configure(IConfiguration config)
        => Configure(config, ConsoleLogger.Default);
    public static IExport Configure(IConfiguration config, ILogger logger)
    {
        logger.WriteLine(); // this helps visually separate us from BenchmarkDotNet's output.
        IConfigurationSection section = config.GetSection(glassViewSection);
        if (!section.Exists()) {
            logger.WriteHint($"No configuration section '{glassViewSection}' found. Using default directory exporter.");
            return Default(logger);
        }
        var export = section.Section<Configuration.Export>();
        var serializationOptions = SerializationOptions().Configure(export.JsonFormatting);
        return export switch {
            { Directory.Path: var directory } => new DirectoryExport(CreateDirectory(directory), serializationOptions, logger),
            _ => throw new ArgumentOutOfRangeException(nameof(config), $"The '{glassViewSection}' section does not contain a valid '{nameof(Export)}' configuration section.")
        };
    }

    private static DirectoryInfo GlassViewArtifactsDirectory() => Extensions.FindLeaf("BenchmarkDotNet.Artifacts").CreateSubdirectory(nameof(GlassView));

    private static JsonSerializerOptions SerializationOptions() => new JsonSerializerOptions().EnableGlassView();
}
