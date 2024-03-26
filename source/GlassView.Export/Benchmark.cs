using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Portability.Cpu;
using BenchmarkDotNet.Reports;
using GlassView.Core;
using GlassView.Core.Models;

namespace GlassView.Export;

public sealed class Benchmark(Summary summary) : IBenchmark, ICreate<Benchmark, Summary>
{
    public required String Name { get; init; }
    public required String FullName { get; init; }
    public Int32 Count => summary.BenchmarksCases.Length;
    public required String Namespace { get; init; }
    public required DateTime TimeStamp { get; init; }
    public TimeSpan Duration => summary.TotalTime;
    public EnvironmentInfo Environment { get; } = new EnvironmentInfo(summary.HostEnvironmentInfo);

    public IEnumerator<IBenchmarkResult> GetEnumerator()
    {
        BenchmarkReport? report;
        foreach (var benchmarkCase in summary.BenchmarksCases) {
            if ((report = summary[benchmarkCase]) != null && report.Success && report.ResultStatistics != null) {
                yield return new BenchmarkResult(benchmarkCase, report.ResultStatistics, report.GcStats);
            }
        }
    }

    public Benchmark Create(Summary argument)
    {
        var titleSegments = argument.Title.Split('-');
        var fullName = titleSegments[0];
        var timestamp = ParseTimestamp(titleSegments[1..]);
        var (@namespace, name) = ParseName(fullName);
        return new Benchmark(argument) {
            Name = name,
            FullName = fullName,
            Namespace = @namespace,
            TimeStamp = timestamp,
        };

        static (String ns, String name) ParseName(String fullName)
        {
            var segments = fullName.Split('.');
            var @namespace = String.Join(String.Empty, segments[..^1]);
            return (@namespace, segments[^1]);
        }
        static DateTime ParseTimestamp(String[] dateTime)
        {
            var date = DateOnly.ParseExact(dateTime[0], "yyMMdd");
            var time = TimeOnly.ParseExact(dateTime[1], "HHmmss");
            return new DateTime(date, time, DateTimeKind.Local);
        }
    }
}


public sealed class EnvironmentInfo(HostEnvironmentInfo environment)
{
    public String OsVersion => environment.OsVersion.Value;
    public DotnetInfo Dotnet { get; } = new DotnetInfo(environment);
    public ProcessorInfo Processor { get; } = new ProcessorInfo(environment.CpuInfo.Value, environment.Architecture);
}

public sealed class DotnetInfo(HostEnvironmentInfo environment)
{
    public Boolean HasRyuJit => environment.HasRyuJit;
    public String BuildConfig => environment.Configuration;
    public String DotNetVersion { get; } = environment.DotNetSdkVersion.Value;
    public Boolean HasAttachedDebugger = environment.HasAttachedDebugger;
}

public sealed class ProcessorInfo(CpuInfo cpuInfo, String arch) : IName
{
    public String Name => cpuInfo.ProcessorName;
    public String Architecture => arch;
    public String HardwareIntrinsics => "ToDo!";
    public Int32 Count { get; } = cpuInfo.PhysicalCoreCount ?? 0;
    public Int32 PhysicalCoreCount { get; } = cpuInfo.PhysicalCoreCount ?? 0;
    public Int32 LogicalCoreCount { get; } = cpuInfo.LogicalCoreCount ?? 0;
}
