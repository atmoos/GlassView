using BenchmarkDotNet.Engines;
using BenchmarkDotNet.Mathematics;
using BenchmarkDotNet.Running;
using GlassView.Core.Models;

namespace GlassView.Export;

public sealed class BenchmarkResult(BenchmarkCase benchmarkCase, Statistics statistics, GcStats gcStats) : IBenchmarkResult
{
    public String Name => benchmarkCase.Descriptor.WorkloadMethod.Name;
    public Boolean IsBaseline = benchmarkCase.Descriptor.Baseline;
    // ToDo: What's the use case for multiple categories?
    public String[] Categories => benchmarkCase.Descriptor.Categories;
    public StatisticsInfo Statistics { get; } = new StatisticsInfo(statistics);
    public AllocationInfo Allocation { get; } = new AllocationInfo(gcStats, gcStats.GetBytesAllocatedPerOperation(benchmarkCase) ?? 0);
}


// ToDo: Should we include statistics.ConfidenceInterval?
public sealed class StatisticsInfo(Statistics statistics)
{
    public Double Mean => statistics.Mean;
    public Double Median => statistics.Median;

    // Can be computed as StdDev/Sqrt(N)!!
    public Double StandardError => statistics.StandardError;
    public Double StandardDeviation => statistics.StandardDeviation;
    public Int32 SampleSize => statistics.N;
}

public sealed class AllocationInfo(GcStats gCStats, Int64 allocatedBytes)
{
    public Int32 Gen0Collections => gCStats.Gen0Collections;
    public Int32 Gen1Collections => gCStats.Gen1Collections;
    public Int32 Gen2Collections => gCStats.Gen2Collections;
    public Int64 AllocatedBytes => allocatedBytes;
}
