namespace GlassView.Core.Models;

public sealed record class BenchmarkCase : IName
{
    public required String Name { get; set; }
    public required Boolean IsBaseline { get; set; }
    public required String[] Categories { get; set; }
    public required StatisticsInfo Statistics { get; set; }
    public required AllocationInfo Allocation { get; set; }
}
