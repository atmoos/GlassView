using System.Text;
using Atmoos.GlassView.Core.Models;

namespace GlassView.Display.Html;

internal sealed class CurrentResults
{
    private readonly BenchmarkSummary summary;

    public CurrentResults(BenchmarkSummary summary)
    {
        this.summary = summary;
    }

    private String BenchmarkInfo()
    {
        var builder = new StringBuilder("<table class='benchInfo'>").Append(Environment.NewLine);
        builder.Append(Common.AddTableRow("Name", this.summary.Name));
        builder.Append(Common.AddTableRow("Count", this.summary.Count.ToString()));
        builder.Append(Common.AddTableRow("Namespace", this.summary.Namespace));
        builder.Append(Common.AddTableRow("Timestamp", this.summary.Timestamp.ToString()));
        builder.Append(Common.AddTableRow("Duration", this.summary.Duration.ToString()));
        builder.Append("</table>");

        return builder.ToString();
    }

    private static String Style => ".benchInfo {border-collapse: collapse;}";

    public String Result => $@"
<html>
{Common.Header(this.summary.Name, Style)}
<body>
{BenchmarkInfo()}
</body>
</html>";
}
