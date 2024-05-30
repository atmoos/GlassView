namespace Atmoos.GlassView.Core.Models;

public sealed record class ProcessorInfo : IName
{
    public required String Name { get; set; }
    public Int32 Count { get; set; }
    public required String Architecture { get; set; }
    public Int32 PhysicalCoreCount { get; set; }
    public Int32 LogicalCoreCount { get; set; }
}
