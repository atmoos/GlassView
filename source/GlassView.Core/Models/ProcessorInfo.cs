namespace GlassView.Core.Models;

public sealed class ProcessorInfo : IName
{
    public required String Name { get; set; }
    public Int32 Count { get; set; }
    public required String Architecture { get; set; }
    public required String HardwareIntrinsics { get; set; }
    public Int32 PhysicalCoreCount { get; set; }
    public Int32 LogicalCoreCount { get; set; }
}