using BenchmarkDotNet.Reports;

namespace GlassView.Export;

public interface IExport
{
    Task Export(Summary inputSummary, CancellationToken token = default);
}
