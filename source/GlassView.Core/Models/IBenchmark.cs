namespace GlassView.Core.Models;

public interface IBenchmark : IName, ICountable<IBenchmarkCase>
{
    String FullName { get; }
    String Namespace { get; }
    DateTime TimeStamp { get; }
    TimeSpan Duration { get; }
    IEnvironmentInfo Environment { get; }
}
