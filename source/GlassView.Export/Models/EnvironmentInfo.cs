using BenchmarkDotNet.Environments;
using GlassView.Core.Models;

namespace GlassView.Export.Models;

public sealed class EnvironmentInfo(HostEnvironmentInfo environment) : IEnvironmentInfo
{
    public String OsVersion => environment.OsVersion.Value;
    public IDotnetInfo Dotnet { get; } = new DotnetInfo(environment);
    public IProcessorInfo Processor { get; } = new ProcessorInfo(environment.CpuInfo.Value, environment.Architecture);
}
