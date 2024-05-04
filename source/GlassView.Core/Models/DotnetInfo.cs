namespace GlassView.Core.Models;

public sealed record class DotnetInfo
{
    public required Boolean HasRyuJit { get; set; }
    public required String BuildConfig { get; set; }
    public required String DotNetVersion { get; set; }
    public required Boolean HasAttachedDebugger { get; set; }
}
