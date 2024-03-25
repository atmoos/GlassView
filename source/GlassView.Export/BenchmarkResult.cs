using System.Collections;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;
using GlassView.Core.Models;

namespace GlassView.Export;

public sealed class BenchmarkResult(BenchmarkCase benchmarkCase) : IBenchmarkResult
{
    public String Name => benchmarkCase.Descriptor.WorkloadMethod.Name;
}
