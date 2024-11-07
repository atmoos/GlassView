namespace GlassView.Display.Html;

internal static class Common
{
    public static String Header(String title, String style = "") => @"
<head>
    <title>{title}</title>
    <style>{style}</style>
</head>
    ";

    public static String AddTableRow(String name, String value) => $@"<tr>
    <td>{name}</td>
    <td>{value}</td>
</tr>";
}
