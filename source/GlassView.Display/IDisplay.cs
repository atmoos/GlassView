using Atmoos.GlassView.Core.Models;

namespace GlassView.Display;

public interface IDisplay
{
   Task Render(IEnumerable<BenchmarkSummary> summaries, Mode displayMode, CancellationToken token = default);
}
