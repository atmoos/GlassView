using BenchmarkDotNet.Reports;

namespace GlassView.Export;

public interface IGlassView
{
    Task Export(Summary summary, CancellationToken token = default);
}
