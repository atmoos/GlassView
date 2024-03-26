namespace GlassView.Core.Models;

public interface IAllocationInfo
{
    Int32 Gen0Collections { get; }
    Int32 Gen1Collections { get; }
    Int32 Gen2Collections { get; }
    Int64 AllocatedBytes { get; }
}
