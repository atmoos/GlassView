using Microsoft.Extensions.Configuration;

namespace Atmoos.GlassView.Export;

public static class GlassView
{
    private const String glassViewSection = nameof(GlassView);

    public static IExport Configure(IConfiguration config)
    {
        IConfigurationSection section = config.GetSection(glassViewSection);
        if (!section.Exists()) {
            throw new ArgumentOutOfRangeException(nameof(config), $"The configuration does not contain a '{glassViewSection}' section.");
        }
        var export = section.Item<Configuration.Export>();
        return export switch {
            { Directory.Path: var directory, JsonFormatting: null } => new DirectoryExport(new DirectoryInfo(directory)),
            { Directory.Path: var directory, JsonFormatting: var formatting } => new DirectoryExport(new DirectoryInfo(directory), formatting),
            _ => throw new ArgumentOutOfRangeException(nameof(config), $"The '{glassViewSection}' section does not contain a valid '{nameof(Export)}' configuration section.")
        };
    }
}
