namespace GlassView.Core.Models;

public interface IStatisticsInfo
{
    Double Mean { get; }
    Double Median { get; }

    // Can be computed as StdDev/Sqrt(N)!!
    Double StandardError { get; }
    Double StandardDeviation { get; }
    Int32 SampleSize { get; }
}
