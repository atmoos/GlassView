
namespace Atmoos.GlassView.Export.Configuration;

/// <summary>
/// Represents the configuration for the GlassView exporter, which
/// is embedded within an application's configuration.
/// </summary>
internal sealed class Export
{
    /// <summary>
    /// Optional configuration for JSON formatting.
    /// </summary>
    public JsonFormatting? JsonFormatting { get; set; }

    /// <summary>
    /// Optional configuration for the directory to export to.
    /// If not specified, a default directory within the working directory is used.
    /// </summary>
    public Directory? Directory { get; set; }
}

internal sealed class Directory
{
    public String Path { get; set; } = String.Empty;
}

internal sealed class JsonFormatting
{
    /// <summary>
    /// Whether to indent the JSON output.
    /// If not specified, System.Text.Json's default is used.
    /// </summary>
    public Boolean? Indented { get; set; }

    /// <summary>
    /// Whether to allow trailing commas in JSON objects and arrays.
    /// If not specified, System.Text.Json's default is used.
    /// </summary>
    public Boolean? AllowTrailingCommas { get; set; }
}
