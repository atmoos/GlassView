namespace GlassView.Core.Models;

public interface IEnvironmentInfo
{
    String OsVersion { get; }
    IDotnetInfo Dotnet { get; }
    IProcessorInfo Processor { get; }
}
