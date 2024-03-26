using System.Collections;

namespace GlassView.Core;

public interface ICountable<T> : IEnumerable<T>
{
    Int32 Count { get; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
