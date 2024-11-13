namespace Atmoos.GlassView.Core.Models;

public sealed record class Parameter
{
    public required String Name { get; init; }
    public required String Value { get; init; }
    public required String Type { get; init; }
}
