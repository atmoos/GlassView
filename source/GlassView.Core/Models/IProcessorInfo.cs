namespace GlassView.Core.Models;

public interface IProcessorInfo : IName
{
    Int32 Count { get; }
    String Architecture { get; }
    String HardwareIntrinsics { get; }
    Int32 PhysicalCoreCount { get; }
    Int32 LogicalCoreCount { get; }
}
