using BenchmarkDotNet.Portability.Cpu;
using GlassView.Core.Models;

namespace GlassView.Export.Models;

public sealed class ProcessorInfo(CpuInfo cpuInfo, String arch) : IProcessorInfo
{
    public String Name => cpuInfo.ProcessorName;
    public String Architecture => arch;
    public String HardwareIntrinsics => "ToDo!";
    public Int32 Count { get; } = cpuInfo.PhysicalCoreCount ?? 0;
    public Int32 PhysicalCoreCount { get; } = cpuInfo.PhysicalCoreCount ?? 0;
    public Int32 LogicalCoreCount { get; } = cpuInfo.LogicalCoreCount ?? 0;
}
