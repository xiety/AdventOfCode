using System.Collections.Immutable;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Advent.Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class MissingMapToAttributeAnalyzer : DiagnosticAnalyzer
{
    public const string DiagnosticId = "REGEX002";

    private static readonly DiagnosticDescriptor Rule = new(
        DiagnosticId, "Add [MapTo] attribute", "Add [MapTo<{0}>] to '{1}'", "Usage", DiagnosticSeverity.Warning, isEnabledByDefault: true);

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => [Rule];

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();
        context.RegisterSyntaxNodeAction(AnalyzeInvocation, SyntaxKind.InvocationExpression);
    }

    private void AnalyzeInvocation(SyntaxNodeAnalysisContext context)
    {
        if (!RegexMapAnalyzer.TryGetRegexExtensionInfo(context.Node, context.SemanticModel,
            out var extensionSym, out var regexSym, out var memberAccess))
            return;

        if (!RegexMapAnalyzer.HasMapToAttribute(regexSym))
        {
            if (extensionSym.TypeArguments.Length == 0)
                return;

            var typeArg = extensionSym.TypeArguments[0];
            var props = ImmutableDictionary<string, string?>.Empty
                .Add("TypeName", typeArg.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat));

            var diagnostic = Diagnostic.Create(Rule, memberAccess.Name.GetLocation(), props, typeArg.Name, regexSym.Name);
            context.ReportDiagnostic(diagnostic);
        }
    }
}
