using BenchmarkDotNet.Reports;

namespace GlassView.Export;

public interface IExport
{
    Task Export(Summary summary, CancellationToken token = default);
}
