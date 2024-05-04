namespace GlassView.Core.Models;

public sealed record class AllocationInfo
{
    public required Int32 Gen0Collections { get; set; }
    public required Int32 Gen1Collections { get; set; }
    public required Int32 Gen2Collections { get; set; }
    public required Int64 AllocatedBytes { get; set; }
}
