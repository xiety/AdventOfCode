using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Advent.Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class PreferToArrayExtensionAnalyzer : DiagnosticAnalyzer
{
    public const string DiagnosticId = "ADVENT001";

    static readonly DiagnosticDescriptor Rule = new(
        id: DiagnosticId,
        title: "Prefer ToArray extension method over Select(...).ToArray()",
        messageFormat: "Use 'nodes.ToArray(a => a.Value)' instead of 'nodes.Select(a => a.Value).ToArray()'",
        category: "Performance",
        DiagnosticSeverity.Warning,
        isEnabledByDefault: true,
        description: "Using the ToArray<TSource,TResult>(selector) extension improves readability and performance.");

    static readonly string[] TargetMethods = ["ToArray", "ToList"];

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
        => ImmutableArray.Create(Rule);

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();

        context.RegisterSyntaxNodeAction(AnalyzeInvocation, SyntaxKind.InvocationExpression);
    }

    void AnalyzeInvocation(SyntaxNodeAnalysisContext context)
    {
        var invocation = (InvocationExpressionSyntax)context.Node;

        if (TryMatchSelectToTargetMethod(invocation, context.SemanticModel, context.CancellationToken,
            out _, out _, out var methodName))
        {
            var diagnostic = Diagnostic.Create(Rule, invocation.GetLocation(),
                messageArgs: new[] { methodName });
            context.ReportDiagnostic(diagnostic);
        }
    }

    public static bool TryMatchSelectToTargetMethod(
        InvocationExpressionSyntax invocation,
        SemanticModel semanticModel,
        CancellationToken ct,
        [NotNullWhen(true)] out ExpressionSyntax? collection,
        [NotNullWhen(true)] out ExpressionSyntax? selector,
        [NotNullWhen(true)] out string? targetMethodName)
    {
        collection = null;
        selector = null;
        targetMethodName = null;

        if (invocation.Expression is not MemberAccessExpressionSyntax outerAccess ||
            invocation.ArgumentList.Arguments.Count != 0)
            return false;

        targetMethodName = outerAccess.Name.Identifier.ValueText;
        if (!TargetMethods.Contains(targetMethodName))
            return false;

        if (outerAccess.Expression is not InvocationExpressionSyntax selectCall ||
            selectCall.Expression is not MemberAccessExpressionSyntax selectAccess ||
            selectCall.ArgumentList.Arguments.Count != 1)
            return false;

        if (selectAccess.Name.Identifier.ValueText != "Select")
            return false;

        var arg = selectCall.ArgumentList.Arguments[0].Expression;

        if (arg is LambdaExpressionSyntax l && !HasSingleParameter(l))
            return false;

        var convertedType = semanticModel.GetTypeInfo(arg, ct).ConvertedType as INamedTypeSymbol;
        if (convertedType?.DelegateInvokeMethod?.Parameters.Length != 1)
            return false;

        collection = selectAccess.Expression;
        selector = arg;
        return true;
    }

    static bool HasSingleParameter(LambdaExpressionSyntax lambda)
        => lambda switch
        {
            SimpleLambdaExpressionSyntax s => true,
            ParenthesizedLambdaExpressionSyntax p => p.ParameterList.Parameters.Count == 1,
            _ => false
        };
}
