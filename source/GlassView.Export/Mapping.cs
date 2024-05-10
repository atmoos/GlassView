using System.Diagnostics.CodeAnalysis;
using BenchmarkDotNet.Engines;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Mathematics;
using BenchmarkDotNet.Portability.Cpu;
using BenchmarkDotNet.Reports;
using GlassView.Core.Models;

namespace GlassView.Export;

[ExcludeFromCodeCoverage(Justification =
"""
BenchmarkDotNet does not allow for deserialization of Summary objects.
Hence, creating a useful summary instance for testing is a non trivial error
prone task in itself, making a test flaky at best.
""")]
internal static class Mapping
{
    public static BenchmarkSummary Map(Summary summary)
    {
        var titleSegments = summary.Title.Split('-');
        var timestamp = ParseTimestamp(titleSegments[1..]);
        var (@namespace, name) = ParseName(titleSegments[0]);

        return new BenchmarkSummary(Cases(summary)) {
            Name = name,
            Namespace = @namespace,
            Timestamp = timestamp,
            Duration = summary.TotalTime,
            Count = summary.BenchmarksCases.Length,
            Environment = Map(summary.HostEnvironmentInfo)
        };

        static (String ns, String name) ParseName(String fullName)
        {
            var namespaceEnd = fullName.LastIndexOf('.');
            return (fullName[..namespaceEnd], fullName[(namespaceEnd + 1)..]);
        }

        static DateTime ParseTimestamp(String[] dateTime)
        {
            var date = DateOnly.ParseExact(dateTime[0], "yyyyMMdd");
            var time = TimeOnly.ParseExact(dateTime[1], "HHmmss");
            return new DateTime(date, time, DateTimeKind.Local).ToUniversalTime();
        }

        static IEnumerable<BenchmarkCase> Cases(Summary summary)
        {
            BenchmarkReport? report;
            foreach (var benchmarkCase in summary.BenchmarksCases) {
                if ((report = summary[benchmarkCase]) != null && report.Success && report.ResultStatistics != null) {
                    yield return Map(benchmarkCase, report.ResultStatistics, report.GcStats);
                }
            }
        }
    }

    private static BenchmarkCase Map(BenchmarkDotNet.Running.BenchmarkCase benchmarkCase, Statistics statistics, GcStats gcStats)
        => new() {
            Name = benchmarkCase.Descriptor.WorkloadMethod.Name,
            IsBaseline = benchmarkCase.Descriptor.Baseline,
            Categories = benchmarkCase.Descriptor.Categories,
            Allocation = Map(gcStats, gcStats.GetBytesAllocatedPerOperation(benchmarkCase) ?? 0),
            Statistics = Map(statistics)
        };

    private static AllocationInfo Map(GcStats gcStats, Int64 allocatedBytes) => new() {
        Gen0Collections = gcStats.Gen0Collections,
        Gen1Collections = gcStats.Gen1Collections,
        Gen2Collections = gcStats.Gen2Collections,
        AllocatedBytes = allocatedBytes,
    };

    private static StatisticsInfo Map(Statistics statistics) => new() {
        Mean = statistics.Mean,
        Median = statistics.Median,
        StandardDeviation = statistics.StandardDeviation,
        SampleSize = statistics.N,
    };

    private static EnvironmentInfo Map(HostEnvironmentInfo environment) => new() {

        OsVersion = environment.OsVersion.Value,
        Processor = Map(environment.CpuInfo.Value, environment.Architecture),
        Dotnet = MapDotnet(environment)
    };

    private static ProcessorInfo Map(CpuInfo cpuInfo, String architecture) => new() {
        Name = cpuInfo.ProcessorName,
        Architecture = architecture,
        HardwareIntrinsics = "ToDo!",
        Count = cpuInfo.PhysicalCoreCount ?? 0,
        PhysicalCoreCount = cpuInfo.PhysicalCoreCount ?? 0,
        LogicalCoreCount = cpuInfo.LogicalCoreCount ?? 0
    };

    private static DotnetInfo MapDotnet(HostEnvironmentInfo environment) => new() {
        HasRyuJit = environment.HasRyuJit,
        BuildConfig = environment.Configuration,
        DotNetVersion = environment.DotNetSdkVersion.Value,
        HasAttachedDebugger = environment.HasAttachedDebugger
    };
}

