using BenchmarkDotNet.Engines;
using GlassView.Core.Models;

namespace GlassView.Export.Models;

public sealed class AllocationInfo(GcStats gCStats, Int64 allocatedBytes) : IAllocationInfo
{
    public Int32 Gen0Collections => gCStats.Gen0Collections;
    public Int32 Gen1Collections => gCStats.Gen1Collections;
    public Int32 Gen2Collections => gCStats.Gen2Collections;
    public Int64 AllocatedBytes => allocatedBytes;
}
