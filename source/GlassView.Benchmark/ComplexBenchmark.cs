using System.Numerics;
using BenchmarkDotNet.Attributes;

namespace Atmoos.GlassView.Benchmark;

/* This is only a dummy benchmark for us to play around with exports. */

[ShortRunJob]
[IterationCount(7)]
[MemoryDiagnoser]
public class ComplexBenchmark
{
    private Double[][] left, right;
    [Params(54, 42)]
    public Int32 Rows { get; set; }
    [Params(12, 34, 123)]
    public Int32 Center { get; set; }

    [Params(21, 76)]
    public Int32 Cols { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        this.left = Matrix(Rows, Center);
        // As they're arrays, the dimensions looks the wrong
        // way round on the right hand side matrix...
        this.right = Matrix(Cols, Center);
    }


    [Benchmark(Baseline = true), BenchmarkCategory("Vectorised")]
    public Object VectorisedMultiplication() => MatrixProduct(this.left, this.right, VectorisedDotProduct);

    [Benchmark, BenchmarkCategory("Regular")]
    public Object RegularMultiplication() => MatrixProduct(this.left, this.right, DotProduct);

    private static Double[] Vec(Int32 count)
        => Enumerable.Range(0, count).Select(i => i - count / 2d).ToArray();
    private static Double[][] Matrix(Int32 rows, Int32 cols)
        => Enumerable.Range(0, rows).Select(_ => Vec(cols)).ToArray();


    public static Double DotProduct(Double[] left, Double[] right)
    {
        var sum = 0d;
        for (Int32 i = 0; i < left.Length; ++i) {
            sum += left[i] * right[i];
        }
        return sum;
    }

    public static Double VectorisedDotProduct(Double[] left, Double[] right)
    {
        Int32 index = 0;
        var sum = Vector<Double>.Zero;
        var stride = Vector<Double>.Count;

        for (; index < left.Length - stride; index += stride) {
            sum += new Vector<Double>(left, index) * new Vector<Double>(right, index);
        }
        Double finalSum = Vector.Sum(sum);
        for (; index < left.Length; ++index) {
            finalSum += left[index] * right[index];
        }
        return finalSum;
    }

    public static Double[][] MatrixProduct(Double[][] left, Double[][] right, Func<Double[], Double[], Double> dotProduct)
    {
        var result = new Double[left.Length][];
        for (var row = 0; row < left.Length; ++row) {
            var lRow = left[row];
            var resultRow = new Double[right.Length];
            for (var col = 0; col < right.Length; ++col) {
                resultRow[col] = dotProduct(lRow, right[col]);
            }
            result[row] = resultRow;
        }
        return result;
    }
}
