using System.Collections;

namespace Atmoos.GlassView.Core;

public interface ICountable<T> : IEnumerable<T>
{
    Int32 Count { get; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
