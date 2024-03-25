namespace GlassView.Core.Models;
public interface IBenchmarkResult : IName
{

}

public interface IBenchmarkResult<out TResult> : IBenchmarkResult
{
    TResult Result { get; }
    TResult StandardDeviation { get; }
}
