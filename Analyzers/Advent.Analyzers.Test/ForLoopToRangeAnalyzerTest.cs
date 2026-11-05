using Microsoft.CodeAnalysis.Testing;

using CSharpTest = Microsoft.CodeAnalysis.CSharp.Testing.CSharpCodeFixTest<
    Advent.Analyzers.ForLoopToRangeAnalyzer,
    Advent.Analyzers.ForLoopToRangeCodeFix,
    Microsoft.CodeAnalysis.Testing.DefaultVerifier>;

using VerifyCS = Microsoft.CodeAnalysis.CSharp.Testing.CSharpCodeFixVerifier<
    Advent.Analyzers.ForLoopToRangeAnalyzer,
    Advent.Analyzers.ForLoopToRangeCodeFix,
    Microsoft.CodeAnalysis.Testing.DefaultVerifier>;

namespace Advent.Analyzers.Tests;

[TestClass]
public class ForLoopToRangeAnalyzerTests
{
    [TestMethod]
    public async Task For_Number_TriggersAndFixes() =>
        await RunAsync(
            """
            {|#0:for|} (var i = 0; i < 10; ++i)
            {
                Console.WriteLine(i);
            }
            """,
            """
            foreach (var i in 10)
            {
                Console.WriteLine(i);
            }
            """
        );

    [TestMethod]
    public async Task For_Range_TriggersAndFixes() =>
        await RunAsync(
            """
            {|#0:for|} (var i = 5; i < 15; ++i)
            {
                Console.WriteLine(i);
            }
            """,
            """
            foreach (var i in 5..15)
            {
                Console.WriteLine(i);
            }
            """
        );

    [TestMethod]
    public async Task For_RangeMath_TriggersAndFixes() =>
        await RunAsync(
            """
            var a = 20;
            {|#0:for|} (var i = a - 5; i < a + 15; ++i)
            {
                Console.WriteLine(i);
            }
            """,
            """
            var a = 20;
            foreach (var i in (a - 5)..(a + 15))
            {
                Console.WriteLine(i);
            }
            """
        );

    [TestMethod]
    public async Task For_Mutation_Skipped() =>
        await RunAsync(
            """
            for (var i = 0; i < 10; ++i)
            {
                Console.WriteLine(i);
                i++;
            }
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
