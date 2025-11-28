using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Formatting;

using System.Collections.Immutable;
using System.Composition;

namespace Advent.Analyzers;

[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(PreferToArrayExtensionCodeFixProvider)), Shared]
public sealed class PreferToArrayExtensionCodeFixProvider : CodeFixProvider
{
    private const string Title = "Use ToArray(selector) / ToList(selector) extension";

    public override ImmutableArray<string> FixableDiagnosticIds
        => [PreferToArrayExtensionAnalyzer.DiagnosticId];

    public override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

    public override async Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
        if (root is null)
            return;

        var diagnostic = context.Diagnostics.First();
        var diagnosticSpan = diagnostic.Location.SourceSpan;

        var invocation = root.FindToken(diagnosticSpan.Start).Parent?
            .AncestorsAndSelf()
            .OfType<InvocationExpressionSyntax>()
            .FirstOrDefault(i => i.Span.Contains(diagnosticSpan));

        if (invocation is null)
            return;

        if (PreferToArrayExtensionAnalyzer.TryMatchSelectToTargetMethod(
                invocation,
                out var collection,
                out var selectorLambda,
                out var methodName))
        {
            context.RegisterCodeFix(
                CodeAction.Create(
                    title: Title,
                    createChangedDocument: ct => ApplyFixAsync(context.Document, invocation, collection!, selectorLambda!, methodName!, ct),
                    equivalenceKey: Title),
                context.Diagnostics.First());
        }
    }

    private static async Task<Document> ApplyFixAsync(
        Document document,
        InvocationExpressionSyntax oldInvocation,
        ExpressionSyntax collection,
        LambdaExpressionSyntax selectorLambda,
        string methodName,
        CancellationToken ct)
    {
        var newInvocation = SyntaxFactory.InvocationExpression(
            SyntaxFactory.MemberAccessExpression(
                SyntaxKind.SimpleMemberAccessExpression,
                collection,
                SyntaxFactory.IdentifierName(methodName)))
            .WithArgumentList(SyntaxFactory.ArgumentList(
                SyntaxFactory.SingletonSeparatedList(SyntaxFactory.Argument(selectorLambda))))
            .WithAdditionalAnnotations(Formatter.Annotation);

        var root = await document.GetSyntaxRootAsync(ct).ConfigureAwait(false);
        return document.WithSyntaxRoot(root!.ReplaceNode(oldInvocation, newInvocation));
    }
}
