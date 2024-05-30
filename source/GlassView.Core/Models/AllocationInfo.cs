namespace Atmoos.GlassView.Core.Models;

public sealed record class AllocationInfo
{
    public required Int32 Gen0Collections { get; init; }
    public required Int32 Gen1Collections { get; init; }
    public required Int32 Gen2Collections { get; init; }
    public required Int64 AllocatedBytes { get; init; }
}
