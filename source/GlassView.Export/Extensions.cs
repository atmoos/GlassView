using System.Text.Json;
using Atmoos.GlassView.Core;

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

    internal static String Serialize<T>(this T value) => Serialize(value, Options(new JsonSerializerOptions()));
    internal static String Serialize<T>(this T value, JsonSerializerOptions options) => JsonSerializer.Serialize(value, options);

    private static JsonSerializerOptions Options(JsonSerializerOptions options)
    {
        options.WriteIndented = true;
        return options.EnableGlassView();
    }

    /// <summary>
    /// Recursively looks upward toward parent directories for the leaf directory <paramref name="leafDirectoryName"/>
    /// in the current directory structure staring at the current working directory.
    /// </summary>
    /// <param name="leafDirectoryName">The leaf directories name.</param>
    /// <returns>If found, returns that directory.
    /// When not found, returns the leaf directory as subdirectory of the current directory.</returns>
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
    internal static void SetValue<T>(T? value, Action<T> setter)
        where T : struct
    {
        if (value != null) {
            setter(value.Value);
        }
    }

    internal static void Set<T>(T? value, Action<T> setter)
        where T : class
    {
        if (value != null) {
            setter(value);
        }
    }
}
