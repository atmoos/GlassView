using static Atmoos.GlassView.Export.Extensions;

namespace Atmoos.GlassView.Export.Test;

public class ExtensionsTest
{

    [Fact]
    public void FindLeafFindsExistingLeafDirInSomeToplevelDirectory()
    {
        const String directoryName = "someExistingLeafDir";
        DirectoryInfo expected = MoveUp(CurrentDirectory(), levels: 3).CreateSubdirectory(directoryName);
        try {
            DirectoryInfo actual = FindLeaf(directoryName);

            Assert.Equal(expected.FullName, actual.FullName);
            Assert.True(expected.Exists, "The expected directory should exist after the test.");
        }
        finally {
            expected.Delete(true);
        }
    }

    [Fact]
    public void FindLeafReturnsCreatedLeafDirInCurrentDirectoryWhenItCannotBeFound()
    {
        const String directoryName = "someLeafDirThatDoesNotExist";
        var expected = new DirectoryInfo(Path.Combine(CurrentDirectory().FullName, directoryName));
        Assert.False(expected.Exists, "The expected directory should not exist before the test.");

        try {

            DirectoryInfo actual = FindLeaf(directoryName);

            Assert.Equal(expected.FullName, actual.FullName);
            Assert.True(actual.Exists, "The expected directory should exist after the test.");
        }
        finally {
            expected.Delete(true);
        }
    }

    private static DirectoryInfo MoveUp(DirectoryInfo directory, Int32 levels)
    {
        for (; directory.Parent != null && levels-- > 0; directory = directory.Parent) { }
        return directory;
    }
}
