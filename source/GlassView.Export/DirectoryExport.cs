using System.Text.Json;
using BenchmarkDotNet.Loggers;
using Atmoos.World;
using Atmoos.GlassView.Core.Models;

namespace Atmoos.GlassView.Export;

internal sealed class DirectoryExport<FileSystem>(IDirectory directory, JsonSerializerOptions options) : IExport
    where FileSystem : IFileCreation
{
    public async Task Export(BenchmarkSummary summary, ILogger logger, CancellationToken token)
    {
        IFile file = FileSystem.Create(directory, FileNameFor(summary));
        logger.WriteLineInfo($"file: {file}");
        using var stream = file.OpenWrite();
        await JsonSerializer.SerializeAsync(stream, summary, options, token).ConfigureAwait(ConfigureAwaitOptions.None);
    }

    public override String ToString() => $"{nameof(Export)}: {directory.ToPath()}";
    private static FileName FileNameFor(BenchmarkSummary summary)
        => new($"{summary.Name}-{summary.Timestamp.ToLocalTime():s}".Replace(':', '-'), "json");
}
