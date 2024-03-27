using BenchmarkDotNet.Reports;

namespace GlassView.Export;

public interface IExporter
{
    Task Export(Summary summary, CancellationToken token = default);
}
