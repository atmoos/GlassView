using BenchmarkDotNet.Reports;

namespace Atmoos.GlassView.Export;

public interface IExport
{
    Task Export(Summary inputSummary, CancellationToken token = default);
}
