namespace GlassView.Core.Models;

public sealed record class StatisticsInfo
{
    public required Double Mean { get; set; }
    public required Double Median { get; set; }
    public Double StandardError => StandardDeviation / Math.Sqrt(SampleSize);
    public required Double StandardDeviation { get; set; }
    public required Int32 SampleSize { get; set; }
}
