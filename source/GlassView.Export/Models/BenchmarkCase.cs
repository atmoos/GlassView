using BenchmarkDotNet.Engines;
using BenchmarkDotNet.Mathematics;
using GlassView.Core.Models;

namespace GlassView.Export.Models;

public sealed class BenchmarkCase(BenchmarkDotNet.Running.BenchmarkCase benchmarkCase, Statistics statistics, GcStats gcStats) : IBenchmarkCase
{
    public String Name => benchmarkCase.Descriptor.WorkloadMethod.Name;
    public Boolean IsBaseline => benchmarkCase.Descriptor.Baseline;
    // ToDo: What's the use case for multiple categories?
    public String[] Categories => benchmarkCase.Descriptor.Categories;
    public IStatisticsInfo Statistics { get; } = new StatisticsInfo(statistics);
    public IAllocationInfo Allocation { get; } = new AllocationInfo(gcStats, gcStats.GetBytesAllocatedPerOperation(benchmarkCase) ?? 0);
}
