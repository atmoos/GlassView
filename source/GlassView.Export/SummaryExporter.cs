using Atmoos.GlassView.Core;
using Atmoos.GlassView.Core.Models;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Reports;

using static Atmoos.GlassView.Export.Mapping;
using static System.Threading.Tasks.ConfigureAwaitOptions;

namespace Atmoos.GlassView.Export;

internal sealed class SummaryExporter(List<IExport> exporters, ILogger logger) : IGlassView, ICountable<IExport>
{
    public Int32 Count => exporters.Count;
    internal SummaryExporter(IExport exporter, ILogger logger) : this([exporter], logger) { }

    public async Task Export(Summary inputSummary, CancellationToken token)
    {
        logger.WriteLine();
        var summary = Map(inputSummary);
        logger.WriteLineHeader($"// * Exporting '{summary.Name}' *");
        await Export(summary, new ExportLogger(logger), token).ConfigureAwait(None);
    }

    public async Task Export(IEnumerable<Summary> inputSummaries, CancellationToken token = default)
    {
        logger.WriteLine();
        Task export = Task.CompletedTask;
        var count = inputSummaries.Count(); // BenchmarkDotNet creates an array. Hence, this is O(1).
        var exportLogger = new ExportLogger(logger);
        var summaryText = count == 1 ? "summary" : "summaries";
        logger.WriteLineHeader($"// * Exporting {count} {summaryText} *");
        foreach (Summary inputSummary in inputSummaries) {
            await export.ConfigureAwait(None);
            var summary = Map(inputSummary);
            logger.WriteLineInfo($"- '{summary.Name}' to:");
            export = Export(summary, exportLogger, token);
        }
        await export.ConfigureAwait(None);
    }

    private async Task Export(BenchmarkSummary summary, ILogger logger, CancellationToken token)
    {
        var exportTask = Task.CompletedTask;
        foreach (var exporter in exporters) {
            await exportTask.ConfigureAwait(None);
            exportTask = exporter.Export(summary, logger, token);
        }
        await exportTask.ConfigureAwait(None);
    }

    public IEnumerator<IExport> GetEnumerator() => exporters.GetEnumerator();
}

file sealed class ExportLogger(ILogger logger) : ILogger
{
    public String Id => logger.Id;
    public Int32 Priority => logger.Priority;
    public void Write(LogKind logKind, String text) => logger.Write(logKind, Indent(text));
    public void WriteLine(LogKind logKind, String text) => logger.WriteLine(logKind, Indent(text));
    public void WriteLine() => logger.WriteLine();
    public void Flush() => logger.Flush();
    private static String Indent(String text) => $" -> {text}";
}
