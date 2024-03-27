namespace GlassView.Core.Models;

public interface IBenchmark : IName, ICountable<IBenchmarkCase>
{
    String Namespace { get; }
    DateTime Timestamp { get; }
    TimeSpan Duration { get; }
    IEnvironmentInfo Environment { get; }
}
