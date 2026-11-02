using System.Reflection;

using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Exporters.Csv;
using BenchmarkDotNet.Exporters.Json;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Toolchains.InProcess.Emit;

var config = ManualConfig.CreateEmpty()
    .AddDiagnoser(MemoryDiagnoser.Default)
    .AddJob(Job.Default.WithToolchain(InProcessEmitToolchain.Instance))
    .AddColumnProvider(DefaultColumnProviders.Instance)
    .AddLogger(ConsoleLogger.Default)
    .AddExporter(CsvExporter.Default)
    .AddExporter(JsonExporter.Default);

var assemblies = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.Problems.dll")
    .ToArray(Assembly.LoadFrom);

BenchmarkSwitcher.FromAssemblies(assemblies).Run(args, config);
