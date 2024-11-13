using BenchmarkDotNet.Loggers;
using Xunit.Abstractions;

namespace Atmoos.GlassView.Export.Test;

public class TestLogger(ITestOutputHelper output) : ILogger
{
    public String Id => nameof(TestLogger);
    public Int32 Priority => 0;
    public void Flush() { }
    public void Write(LogKind logKind, String text) => WriteLine(logKind, text);
    public void WriteLine() => output.WriteLine("");
    public void WriteLine(LogKind logKind, String text) => output.WriteLine($"{logKind}: {text}");
}
