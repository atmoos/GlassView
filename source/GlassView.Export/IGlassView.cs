using BenchmarkDotNet.Reports;

namespace Atmoos.GlassView.Export;

public interface IGlassView
{
    Task Export(Summary inputSummary, CancellationToken token = default);
    Task Export(IEnumerable<Summary> inputSummaries, CancellationToken token = default);
}
