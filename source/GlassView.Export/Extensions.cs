using System.Text.Json;
using BenchmarkDotNet.Reports;
using Atmoos.GlassView.Core;
using Microsoft.Extensions.Configuration;
using static System.Threading.Tasks.ConfigureAwaitOptions;

namespace Atmoos.GlassView.Export;

public static class Extensions
{
    private static readonly EnumerationOptions findLeafOptions = new() {
        RecurseSubdirectories = false,
        IgnoreInaccessible = true,
        MatchType = MatchType.Simple,
        MatchCasing = MatchCasing.CaseSensitive,
        AttributesToSkip = FileAttributes.Hidden | FileAttributes.System
    };

    public static async Task Export(this IExport exporter, IEnumerable<Summary> inputSummaries, CancellationToken token = default)
    {
        // For fun, some interleaved exporting :-)
        Task export = Task.CompletedTask;
        foreach (Summary summary in inputSummaries) {
            Task next = exporter.Export(summary, token);
            await export.ConfigureAwait(None);
            export = next;
        }
        await export.ConfigureAwait(None);
    }

    internal static String Serialize<T>(this T value) => Serialize(value, Options(new JsonSerializerOptions()));
    internal static String Serialize<T>(this T value, JsonSerializerOptions options) => JsonSerializer.Serialize(value, options);

    private static JsonSerializerOptions Options(JsonSerializerOptions options)
    {
        options.WriteIndented = true;
        return options.EnableGlassView();
    }

    /// <summary>
    /// Find the leaf directory in the current directory structure.
    /// </summary>
    /// <param name="leafDirectoryName">The leaf directories name.</param>
    /// <returns>If found returns that directory.
    /// When not found returns the leaf directory as subdirectory of the current directory.</returns>
    internal static DirectoryInfo FindLeaf(String leafDirectoryName)
    {
        DirectoryInfo? result;
        DirectoryInfo cwd, directory;
        directory = cwd = new DirectoryInfo(Directory.GetCurrentDirectory());
        while ((result = directory.EnumerateDirectories(leafDirectoryName, findLeafOptions).SingleOrDefault()) == null) {
            if (directory.Parent == null) {
                return cwd.CreateSubdirectory(leafDirectoryName);
            }
            directory = directory.Parent;
        }
        return result;
    }

    internal static DirectoryInfo CurrentDirectory() => new(Directory.GetCurrentDirectory());
    internal static FileInfo AddFile(this DirectoryInfo directory, String fileName) => new(Path.Combine(directory.FullName, fileName));
}
