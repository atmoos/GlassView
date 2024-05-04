namespace GlassView.Core.Models;

public sealed record class DotnetInfo
{
    public required Boolean HasRyuJit { get; init; }
    public required String BuildConfig { get; init; }
    public required String DotNetVersion { get; init; }
    public required Boolean HasAttachedDebugger { get; init; }
}
