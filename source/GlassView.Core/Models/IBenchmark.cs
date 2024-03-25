using System.Collections;

namespace GlassView.Core.Models;

public interface IBenchmark : IName, IEnumerable<IBenchmarkResult>
{
    DateTime TimeStamp { get; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
