namespace GlassView.Core.Models;
public interface IBenchmarkCase : IName
{
    Boolean IsBaseline { get; }
    String[] Categories { get; }
    IStatisticsInfo Statistics { get; }
    IAllocationInfo Allocation { get; }
}
