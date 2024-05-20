# GlassView

A solution to view and keep track of benchmarking results of dotnet projects.

This is still a project very much in it's infancy. But when it matures, it is supposed to export [BenchmarkDotNet](https://github.com/dotnet/BenchmarkDotNet) results such that they can be tracked over time. This should - if all goes according to plans -- allow teams to keep track of the performance characteristics of a project. Just as many teams already do with correctness and quality aspects by keeping trach of test results (coverage) and code quality metrics.

## Vision

The end product should be some interactive web interface that can run "anywhere" which enables analysis of your benchmark results.

- [ ] Export benchmark results as json to some local directory.
  - [x] Via a library (ToDo #12: nuget pending)
  - [ ] Completely decoupled without any dll dependencies
- [ ] Ingestion into some service(?) that creates (static) views.
- [ ] Interactive web interface.
- [ ] IDE integration (Visual Studio Code)?
- [ ] Statistics suite with which,
  - [ ] simple queries against past runs can be issued.
  - [ ] regressions and trends can be detected and visualised
- [ ] Integration into Ci/Cd systems (pipelines)

For a complete and more detailed list, please check out our [issues](https://github.com/atmoos/GlassView/issues).

## Current Set-Up

Reference the nuget package `Atmoos.GlassView.Export` and modify your benchmarking project as follows:

```csharp
/* in: YourProject/Program.cs */
// other usings...
using Atmoos.GlassView.Export;
using Microsoft.Extensions.Configuration;

var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

IGlassView exporter = GlassView.Configure(configuration);

// dotnet run -c Release --project 'YourProject/'
var summary = BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);

await exporter.Export(summary);
```

## Configuration

[Configuration](https://github.com/atmoos/GlassView/blob/main/source/GlassView.Export/Configuration/Configuration.cs) allows for optional configuration of:

- The export directory.
- Json formatting options.

Without any configuration, the benchmark results are exported to `./BenchmarkDotNet.Artifacts/GlassView/`.

A minimal `GlassView` configuration section might look something like this:

```json
{
  "GlassView": {
    "Export": {
      "Directory": {
        "Path": "Your/Export/Directory/"
      }
    }
  }
}
```

## Quick Start

If you just want to get going and manually want to check for exported data, you can do so by using a `Program.cs` similar to this:

```csharp
/* in: YourProject/Program.cs */
// other usings...
using Atmoos.GlassView.Export;

IGlassView exporter = GlassView.Default(); // exports to ./BenchmarkDotNet.Artifacts/GlassView/

// dotnet run -c Release --project 'YourProject/'
var summary = BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);

await exporter.Export(summary);
```
