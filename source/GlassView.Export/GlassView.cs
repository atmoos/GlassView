using Microsoft.Extensions.Configuration;
using BenchmarkDotNet.Loggers;

namespace Atmoos.GlassView.Export;

public static class GlassView
{
    private const String glassViewSection = nameof(GlassView);

    public static IExport Default() => Default(ConsoleLogger.Default);
    public static IExport Default(ILogger logger)
        // ToDo: use benchmark artifact directory.
        => new DirectoryExport(new DirectoryInfo(Directory.GetCurrentDirectory()), logger);

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
        var export = section.Item<Configuration.Export>();
        return export switch {
            { Directory.Path: var directory, JsonFormatting: null } => new DirectoryExport(new DirectoryInfo(directory), logger),
            { Directory.Path: var directory, JsonFormatting: var formatting } => new DirectoryExport(new DirectoryInfo(directory), formatting, logger),
            _ => throw new ArgumentOutOfRangeException(nameof(config), $"The '{glassViewSection}' section does not contain a valid '{nameof(Export)}' configuration section.")
        };
    }
}
