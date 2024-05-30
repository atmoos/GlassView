namespace Atmoos.GlassView.Core.Models;

public sealed record class BenchmarkCase : IName
{
    public required String Name { get; init; }
    public required Boolean IsBaseline { get; init; }
    public required String[] Categories { get; init; }
    public required Parameter[] Parameters { get; init; }
    public required String HardwareIntrinsics { get; init; }
    public required StatisticsInfo Statistics { get; init; }
    public required AllocationInfo Allocation { get; init; }
}
