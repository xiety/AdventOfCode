using System.Text;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace Advent.Gen;

[Generator]
public class TestGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var methods = context.SyntaxProvider
            .CreateSyntaxProvider(
                predicate: (s, _) => s is MethodDeclarationSyntax m && m.AttributeLists.Count > 0,
                transform: (ctx, _) => GetMethodContext(ctx))
            .Where(m => m != null);

        var grouped = methods.Collect();

        context.RegisterSourceOutput(grouped, (spc, sourceInputs) =>
        {
            var inputs = sourceInputs.OfType<MethodData>();
            var byClass = inputs.GroupBy(x => x.SolverClass, SymbolEqualityComparer.Default);

            foreach (var group in byClass)
            {
                var solverClass = (INamedTypeSymbol)group.Key!;
                var code = GenerateTestClass(solverClass, group.ToList());

                var safeName = solverClass.ToDisplayString().Replace("global::", "");
                spc.AddSource($"{safeName}.Tests.g.cs", SourceText.From(code, Encoding.UTF8));
            }
        });
    }

    record MethodData(
        MethodDeclarationSyntax Syntax,
        IMethodSymbol Symbol,
        INamedTypeSymbol SolverClass,
        string Sample,
        string Input,
        bool IsPartA
    );

    private static MethodData? GetMethodContext(GeneratorSyntaxContext ctx)
    {
        var decl = (MethodDeclarationSyntax)ctx.Node;
        var symbol = ctx.SemanticModel.GetDeclaredSymbol(decl);
        if (symbol is null)
            return null;

        var attr = symbol.GetAttributes().FirstOrDefault(a => a.AttributeClass?.Name.StartsWith("GeneratedTest") == true);
        if (attr is null)
            return null;

        var sample = attr.ConstructorArguments[0].ToCSharpString();
        var input = attr.ConstructorArguments[1].ToCSharpString();
        var isA = symbol.Name.StartsWith("RunA", StringComparison.OrdinalIgnoreCase);

        return new MethodData(decl, symbol, symbol.ContainingType, sample, input, isA);
    }

    private static string GenerateTestClass(INamedTypeSymbol solverClass, List<MethodData> methods)
    {
        var ns = solverClass.ContainingNamespace.ToDisplayString();
        var fullClassName = solverClass.ToDisplayString();
        var testClassName = ns.Split('.').Last();

        var sb = new StringBuilder();

        sb.AppendLine($$"""
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Advent.Common;

namespace {{ns}}.Tests;

[TestClass]
public class {{testClassName}} : GeneratedTestBase
{
""");

        foreach (var m in methods)
        {
            var sourcePath = m.Syntax.SyntaxTree.FilePath.Replace(@"\", @"\\");
            sb.AppendLine(GenerateMethodBody(m, fullClassName, sourcePath, isSample: true));
            sb.AppendLine(GenerateMethodBody(m, fullClassName, sourcePath, isSample: false));
        }

        sb.AppendLine("}");
        return sb.ToString();
    }

    private static string GenerateMethodBody(MethodData m, string className, string sourcePath, bool isSample)
    {
        var suffix = isSample ? "Sample" : "Input";
        var expected = isSample ? m.Sample : m.Input;

        var location = m.Syntax.GetLocation();
        var line = location.GetLineSpan().StartLinePosition.Line + 1;
        var displayPath = location.SourceTree?.FilePath ?? "";

        var hasBool = m.Symbol.Parameters.Length == 2;
        var boolArg = isSample ? "true" : "false";
        var args = hasBool ? $"lines, {boolArg}" : "lines";

        var instantiation = "";
        string invocation;

        if (m.Symbol.IsStatic)
        {
            invocation = $"{className}.{m.Symbol.Name}({args})";
        }
        else
        {
            instantiation = $"var solver = new {className}();";
            invocation = $"solver.{m.Symbol.Name}({args})";
        }

        return $$"""

    #line {{line}} "{{displayPath}}"
    [TestMethod]
    public void {{m.Symbol.Name}}_{{suffix}}()
    {
        var lines = LoadInput(@"{{sourcePath}}", {{m.IsPartA.ToString().ToLower()}}, {{boolArg}});
        {{instantiation}}
        var actual = {{invocation}};
        Assert.AreEqual({{expected}}, actual);
    }
    #line default
""";
    }
}
