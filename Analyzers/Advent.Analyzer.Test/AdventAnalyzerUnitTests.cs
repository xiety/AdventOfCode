using Microsoft.CodeAnalysis.Testing;

using CSharpTest = Microsoft.CodeAnalysis.CSharp.Testing.CSharpCodeFixTest<
    Advent.Analyzers.PreferToArrayExtensionAnalyzer,
    Advent.Analyzers.PreferToArrayExtensionCodeFixProvider,
    Microsoft.CodeAnalysis.Testing.DefaultVerifier>;

using VerifyCS = Microsoft.CodeAnalysis.CSharp.Testing.CSharpCodeFixVerifier<
    Advent.Analyzers.PreferToArrayExtensionAnalyzer,
    Advent.Analyzers.PreferToArrayExtensionCodeFixProvider,
    Microsoft.CodeAnalysis.Testing.DefaultVerifier>;

namespace Advent.Analyzers.Tests;

[TestClass]
public class PreferToArrayExtensionAnalyzerTests
{
    [TestMethod]
    public async Task Select_ToArray_TriggersAndFixes() =>
        await RunAsync(
            """
            using System.Linq;
            var items = new[] { "a", "bb", "ccc" };
            var lengths = {|#0:items.Select(s => s.Length).ToArray()|};
            """,
            """
            using System.Linq;
            var items = new[] { "a", "bb", "ccc" };
            var lengths = items.ToArray(s => s.Length);
            """
        );

    [TestMethod]
    public async Task Select_ToList_TriggersAndFixes() =>
        await RunAsync(
            """
            using System.Linq;
            var numbers = Enumerable.Range(1, 5);
            var doubled = {|#0:numbers.Select(n => n * 2).ToList()|};
            """,
            """
            using System.Linq;
            var numbers = Enumerable.Range(1, 5);
            var doubled = numbers.ToList(n => n * 2);
            """
        );

    [TestMethod]
    public async Task ParenthesizedLambda_Works() =>
        await RunAsync(
            """
            using System.Linq;
            var people = new (string Name, int Age)[] { ("Alice", 30) };
            var names = {|#0:people.Select(p => p.Name).ToArray()|};
            """,
            """
            using System.Linq;
            var people = new (string Name, int Age)[] { ("Alice", 30) };
            var names = people.ToArray(p => p.Name);
            """
        );

    [TestMethod]
    public async Task ToArray_WithStateParameter_NoDiagnostic() =>
        await RunAsync(
            """
            using System;
            using System.Linq;

            ReadOnlySpan<char> span = "hello";
            Type state = typeof(string);

            _ = span.EnumerateRunes()
                    .Select(r => char.ConvertFromUtf32(r.Value))
                    .ToArray(state);
            """
        );

    [TestMethod]
    public async Task Select_SeveralParameters_NoDiagnostic() =>
        await RunAsync(
            """
            using System.Linq;
            Enumerable.Range(0, 10)
                .Select((a, i) => a)
                .ToArray();
            """
        );

    [TestMethod]
    public async Task ToArray_WithComparer_NoDiagnostic() =>
        await RunAsync(
            """
            using System;
            using System.Linq;
            using System.Collections.Generic;

            var items = new[] { "a", "B", "c" };
            _ = items.Select(x => x).ToArray(StringComparer.OrdinalIgnoreCase);
            """
        );

    [TestMethod]
    public async Task ToArray_WithAnyArgument_NoDiagnostic() =>
        await RunAsync(
            """
            using System.Linq;

            var items = new[] { 1, 2, 3 };
            _ = items.Select(x => x).ToArray(42);
            _ = items.Select(x => x).ToArray("hello");
            """
        );

    [TestMethod]
    public async Task SingleLine_Call_TriggersAndFixes() =>
        await RunAsync(
            """
            using System.Linq;
            {|#0:Enumerable.Range(1, 10).Select(i => i * i).ToArray()|};
            """,
            """
            using System.Linq;
            Enumerable.Range(1, 10).ToArray(i => i * i);
            """
        );

    static async Task RunAsync(string source, string? fixedSource = null)
    {
        var test = new CSharpTest
        {
            TestCode = source,
            FixedCode = fixedSource ?? source,
            TestState = { OutputKind = Microsoft.CodeAnalysis.OutputKind.ConsoleApplication },
            CompilerDiagnostics = CompilerDiagnostics.None,
        };

        if (fixedSource != null)
            test.ExpectedDiagnostics.Add(VerifyCS.Diagnostic().WithLocation(0));

        await test.RunAsync();
    }
}
