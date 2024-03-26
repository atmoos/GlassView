namespace GlassView.Core.Models;

public interface IDotnetInfo
{
    Boolean HasRyuJit { get; }
    String BuildConfig { get; }
    String DotNetVersion { get; }
    Boolean HasAttachedDebugger { get; }
}
