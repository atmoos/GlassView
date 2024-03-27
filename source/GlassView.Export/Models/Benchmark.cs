using BenchmarkDotNet.Reports;
using GlassView.Core;
using GlassView.Core.Models;

namespace GlassView.Export.Models;

public sealed class Benchmark(Summary summary) : IBenchmark, ICreate<Benchmark, Summary>
{
    public required String Name { get; init; }
    public required String FullName { get; init; }
    public Int32 Count => summary.BenchmarksCases.Length;
    public required String Namespace { get; init; }
    public required DateTime Timestamp { get; init; }
    public TimeSpan Duration => summary.TotalTime;
    public IEnvironmentInfo Environment { get; } = new EnvironmentInfo(summary.HostEnvironmentInfo);

    public IEnumerator<IBenchmarkCase> GetEnumerator()
    {
        BenchmarkReport? report;
        foreach (var benchmarkCase in summary.BenchmarksCases) {
            if ((report = summary[benchmarkCase]) != null && report.Success && report.ResultStatistics != null) {
                yield return new BenchmarkCase(benchmarkCase, report.ResultStatistics, report.GcStats);
            }
        }
    }

    public static Benchmark Create(Summary argument)
    {
        var titleSegments = argument.Title.Split('-');
        var fullName = titleSegments[0];
        var timestamp = ParseTimestamp(titleSegments[1..]);
        var (@namespace, name) = ParseName(fullName);
        return new Benchmark(argument) {
            Name = name,
            FullName = fullName,
            Namespace = @namespace,
            Timestamp = timestamp,
        };

        static (String ns, String name) ParseName(String fullName)
        {
            var segments = fullName.Split('.');
            var @namespace = String.Join('.', segments[..^1]);
            return (@namespace, segments[^1]);
        }
        static DateTime ParseTimestamp(String[] dateTime)
        {
            var date = DateOnly.ParseExact(dateTime[0], "yyyyMMdd");
            var time = TimeOnly.ParseExact(dateTime[1], "HHmmss");
            return new DateTime(date, time, DateTimeKind.Local);
        }
    }
}
