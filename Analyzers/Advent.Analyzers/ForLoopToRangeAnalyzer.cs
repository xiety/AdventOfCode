using System.Collections.Immutable;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Advent.Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class ForLoopToRangeAnalyzer : DiagnosticAnalyzer
{
    public const string DiagnosticId = "ADVENT002";

    static readonly DiagnosticDescriptor Rule = new(
        DiagnosticId,
        "Use foreach with range instead of for-loop",
        "Replace 'for' loop with 'foreach' using a range",
        "Style",
        DiagnosticSeverity.Info,
        true);

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => [Rule];

    public override void Initialize(AnalysisContext ctx)
    {
        ctx.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        ctx.EnableConcurrentExecution();
        ctx.RegisterSyntaxNodeAction(Analyze, SyntaxKind.ForStatement);
    }

    static void Analyze(SyntaxNodeAnalysisContext c)
    {
        var f = (ForStatementSyntax)c.Node;

        if (f.Declaration?.Variables.Count != 1 ||
            f.Initializers.Count != 0 ||
            f.Incrementors.Count != 1)
            return;

        var v = f.Declaration.Variables[0];

        var cv = c.SemanticModel.GetConstantValue(v.Initializer!.Value, c.CancellationToken);
        var isZeroStart = cv is { HasValue: true, Value: 0 or 0L or 0F or 0D or 0M };

        if (f.Condition is not BinaryExpressionSyntax cond)
            return;

        if (isZeroStart)
        {
            var upperType = c.SemanticModel.GetTypeInfo(cond.Right, c.CancellationToken).Type;
            if (upperType is null || !IsNumeric(upperType))
                return;
        }
        else
        {
            var loopSym = c.SemanticModel.GetDeclaredSymbol(v, c.CancellationToken) as ILocalSymbol;
            if (loopSym?.Type.SpecialType != SpecialType.System_Int32)
                return;

            if (cv is { HasValue: true, Value: < 0 })
                return;

            var upperType = c.SemanticModel.GetTypeInfo(cond.Right, c.CancellationToken).Type;
            if (upperType?.SpecialType != SpecialType.System_Int32)
                return;
        }

        if (cond.Kind() != SyntaxKind.LessThanExpression &&
            cond.Kind() != SyntaxKind.LessThanOrEqualExpression)
            return;

        if (cond.Left is not IdentifierNameSyntax id ||
            id.Identifier.ValueText != v.Identifier.ValueText)
            return;

        var inc = f.Incrementors[0];
        if (inc is not (PostfixUnaryExpressionSyntax or PrefixUnaryExpressionSyntax) &&
            !(inc is BinaryExpressionSyntax add && add.Kind() == SyntaxKind.AddAssignmentExpression &&
              add.Right is LiteralExpressionSyntax { Token.Value: 1 }))
            return;

        c.ReportDiagnostic(Diagnostic.Create(Rule, f.ForKeyword.GetLocation()));
    }

    static bool IsNumeric(ITypeSymbol? t)
        => t != null && t.AllInterfaces.Any(
            i => i.Name == "INumber" && i.ContainingNamespace.ToDisplayString() == "System.Numerics");
}
