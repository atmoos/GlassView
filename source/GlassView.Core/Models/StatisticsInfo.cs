namespace Atmoos.GlassView.Core.Models;

public sealed record class StatisticsInfo
{
    public required Double Mean { get; init; }
    public required Double Median { get; init; }
    public Double StandardError => StandardDeviation / Math.Sqrt(SampleSize);
    public required Double StandardDeviation { get; init; }
    public required Int32 SampleSize { get; init; }
}
