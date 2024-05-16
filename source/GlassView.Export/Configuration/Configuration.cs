
namespace Atmoos.GlassView.Export.Configuration;
internal sealed class Export
{
    public JsonFormatting? JsonFormatting { get; set; }
    public Directory? Directory { get; set; }
}

internal sealed class Directory
{
    public String Path { get; set; } = String.Empty;
}

internal sealed class JsonFormatting
{
    public Boolean? Indented { get; set; }
    public Boolean? AllowTrailingCommas { get; set; }
}
