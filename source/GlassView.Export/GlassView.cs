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
        var export = section.Item<Export>();
        return export switch {
            { Directory.Path: var directory } => new DirectoryExport(new DirectoryInfo(directory)),
            _ => throw new ArgumentOutOfRangeException(nameof(config), $"The '{glassViewSection}' section does not contain a valid '{nameof(Export)}' configuration section.")
        };
    }

    private sealed class Export
    {
        public Directory? Directory { get; set; }
    }
    private sealed class Directory
    {
        public String Path { get; set; } = String.Empty;
    }
}
