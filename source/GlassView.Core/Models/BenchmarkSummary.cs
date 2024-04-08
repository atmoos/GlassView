namespace GlassView.Core.Models;

public sealed class BenchmarkSummary(IEnumerable<BenchmarkCase> cases) : IName, ICountable<BenchmarkCase>
{
    public required String Name { get; set; }
    public required Int32 Count { get; set; }
    public required String Namespace { get; set; }
    public required DateTime Timestamp { get; set; }
    public required TimeSpan Duration { get; set; }
    public required EnvironmentInfo Environment { get; set; }
    public IEnumerator<BenchmarkCase> GetEnumerator() => cases.GetEnumerator();
}
