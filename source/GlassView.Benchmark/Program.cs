using System.IO;
using System.Threading.Tasks;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;
using GlassView.Export;

using static BenchmarkDotNet.Columns.StatisticColumn;

IGlassView view = new GlassView.Export.GlassView(Directory.CreateDirectory("test"));

// dotnet run -c Release --project Quantities.Benchmark/
var config = DefaultConfig.Instance.HideColumns(StdDev, Median, Kurtosis, BaselineRatioColumn.RatioStdDev);

var summary = BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args, config);

await view.Export(summary).ConfigureAwait(ConfigureAwaitOptions.None);
