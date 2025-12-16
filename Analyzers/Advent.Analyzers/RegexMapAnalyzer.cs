using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Advent.Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class RegexMapAnalyzer : DiagnosticAnalyzer
{
    public const string DiagnosticId = "REGEX001";

    private static readonly DiagnosticDescriptor Rule = new(
        DiagnosticId, "Use generated wrapper", "Use generated method '{0}'", "Performance", DiagnosticSeverity.Warning, isEnabledByDefault: true);

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => [Rule];

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();
        context.RegisterSyntaxNodeAction(AnalyzeInvocation, SyntaxKind.InvocationExpression);
    }

    private void AnalyzeInvocation(SyntaxNodeAnalysisContext context)
    {
        if (!TryGetRegexExtensionInfo(context.Node, context.SemanticModel,
            out _, out var regexSym, out var memberAccess))
            return;

        if (HasMapToAttribute(regexSym))
        {
            var currentMethodName = memberAccess.Name.Identifier.Text;
            var newMethodName = $"{currentMethodName}{regexSym.Name}";

            var diagnostic = Diagnostic.Create(Rule, memberAccess.Name.GetLocation(), newMethodName);
            context.ReportDiagnostic(diagnostic);
        }
    }

    public static bool TryGetRegexExtensionInfo(
        SyntaxNode node,
        SemanticModel semanticModel,
        [NotNullWhen(true)] out IMethodSymbol? extensionSymbol,
        [NotNullWhen(true)] out IMethodSymbol? regexSourceSymbol,
        [NotNullWhen(true)] out MemberAccessExpressionSyntax? memberAccess)
    {
        extensionSymbol = null;
        regexSourceSymbol = null;
        memberAccess = null;

        if (node is not InvocationExpressionSyntax invocation)
            return false;
        if (invocation.Expression is not MemberAccessExpressionSyntax ma)
            return false;

        memberAccess = ma;

        var methodName = ma.Name.Identifier.Text;

        if (!RegexConstants.SupportedMethods.Contains(methodName))
            return false;

        var symbol = semanticModel.GetSymbolInfo(invocation).Symbol;
        extensionSymbol = symbol as IMethodSymbol;

        if (extensionSymbol is null)
            return false;

        if (extensionSymbol.ContainingType.Name != RegexConstants.ExtensionClassName)
            return false;

        if (ma.Expression is not InvocationExpressionSyntax innerInvocation)
            return false;

        regexSourceSymbol = semanticModel.GetSymbolInfo(innerInvocation).Symbol as IMethodSymbol;
        return regexSourceSymbol is not null;
    }

    public static bool HasMapToAttribute(ISymbol symbol)
    {
        return symbol.GetAttributes()
            .Any(a => a.AttributeClass?.Name == RegexConstants.AttributeName);
    }
}
