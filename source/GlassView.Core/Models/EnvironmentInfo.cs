namespace GlassView.Core.Models;

public sealed class EnvironmentInfo
{
    public required String OsVersion { get; set; }
    public required DotnetInfo Dotnet { get; set; }
    public required ProcessorInfo Processor { get; set; }
}
