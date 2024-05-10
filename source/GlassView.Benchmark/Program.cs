using System.Threading.Tasks;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;
using Microsoft.Extensions.Configuration;
using GlassView.Export;

using static BenchmarkDotNet.Columns.StatisticColumn;

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
    .Build();

IExport exporter = GlassView.Export.GlassView.Configure(configuration);

// dotnet run -c Release --project GlassView.Benchmark/
var config = DefaultConfig.Instance.HideColumns(StdDev, Median, Kurtosis, BaselineRatioColumn.RatioStdDev);

var summary = BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args, config);

await exporter.Export(summary).ConfigureAwait(ConfigureAwaitOptions.None);
