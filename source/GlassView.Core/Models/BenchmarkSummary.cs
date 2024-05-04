namespace GlassView.Core.Models;

public sealed record class BenchmarkSummary(IEnumerable<BenchmarkCase> cases) : IName, ICountable<BenchmarkCase>
{
    public required String Name { get; init; }
    public required Int32 Count { get; init; }
    public required String Namespace { get; init; }
    public required DateTime Timestamp { get; init; }
    public required TimeSpan Duration { get; init; }
    public required EnvironmentInfo Environment { get; init; }
    public IEnumerator<BenchmarkCase> GetEnumerator() => cases.GetEnumerator();
}
