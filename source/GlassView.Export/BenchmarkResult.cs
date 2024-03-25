using System.Collections;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Engines;
using BenchmarkDotNet.Mathematics;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;
using GlassView.Core.Models;

namespace GlassView.Export;

public sealed class BenchmarkResult(BenchmarkCase benchmarkCase, Statistics statistics, GcStats gcStats) : IBenchmarkResult
{
    public String Name => benchmarkCase.Descriptor.WorkloadMethod.Name;
    public Boolean IsBaseline = benchmarkCase.Descriptor.Baseline;
    public Double Mean => statistics.Mean;
    public Double Median => statistics.Median;
    public Double StandardError => statistics.StandardError;
    public Double StandardDeviation => statistics.StandardDeviation;
    public ConfidenceInterval Confidence { get; } = new ConfidenceInterval(statistics.ConfidenceInterval);
}




public sealed class ConfidenceInterval(Perfolizer.Mathematics.Common.ConfidenceInterval confidence)
{
}
