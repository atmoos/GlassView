using BenchmarkDotNet.Environments;
using GlassView.Core.Models;

namespace GlassView.Export.Models;

public sealed class DotnetInfo(HostEnvironmentInfo environment) : IDotnetInfo
{
    public Boolean HasRyuJit => environment.HasRyuJit;
    public String BuildConfig => environment.Configuration;
    public String DotNetVersion { get; } = environment.DotNetSdkVersion.Value;
    public Boolean HasAttachedDebugger => environment.HasAttachedDebugger;
}
