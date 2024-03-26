using BenchmarkDotNet.Mathematics;
using GlassView.Core.Models;

namespace GlassView.Export.Models;

// ToDo: Should we include statistics.ConfidenceInterval?
public sealed class StatisticsInfo(Statistics statistics) : IStatisticsInfo
{
    public Double Mean => statistics.Mean;
    public Double Median => statistics.Median;

    // Can be computed as StdDev/Sqrt(N)!!
    public Double StandardError => statistics.StandardError;
    public Double StandardDeviation => statistics.StandardDeviation;
    public Int32 SampleSize => statistics.N;
}
