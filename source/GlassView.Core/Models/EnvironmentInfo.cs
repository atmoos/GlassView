namespace Atmoos.GlassView.Core.Models;

public sealed record class EnvironmentInfo
{
    public required String OsVersion { get; init; }
    public required DotnetInfo Dotnet { get; init; }
    public required ProcessorInfo Processor { get; init; }
}
