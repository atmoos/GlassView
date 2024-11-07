using Atmoos.GlassView.Core.Models;
using GlassView.Display.Html;

namespace GlassView.Display;

public sealed class Display : IDisplay
{
    public Task Render(IEnumerable<BenchmarkSummary> summaries, Mode displayMode, CancellationToken token)
    {
        switch (displayMode)
        {
            case Mode.HtmlCurrentResults:
                PublishHtml("", new CurrentResults(summaries.First()).Result);
                break;
            default:
                throw new NotImplementedException();
        };

        return Task.CompletedTask;
    }

    private void PublishHtml(String path, String html)
    {

    }
}
