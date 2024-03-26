namespace GlassView.Core.Models;

public interface IBenchmark : IName, ICountable<IBenchmarkResult>
{
    DateTime TimeStamp { get; }
}
