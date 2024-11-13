using Atmoos.GlassView.Core.Models;
using BenchmarkDotNet.Loggers;

namespace Atmoos.GlassView.Export;

internal interface IExport
{
    Task Export(BenchmarkSummary summary, ILogger logger, CancellationToken token);
}

