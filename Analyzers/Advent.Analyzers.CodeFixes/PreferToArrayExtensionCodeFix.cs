using System.Collections.Immutable;
using System.Composition;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Formatting;

namespace Advent.Analyzers;

[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(PreferToArrayExtensionCodeFix)), Shared]
public sealed class PreferToArrayExtensionCodeFix : CodeFixProvider
{
    const string Title = "Use ToArray(selector) / ToList(selector) extension";

    public override ImmutableArray<string> FixableDiagnosticIds
        => [PreferToArrayExtensionAnalyzer.DiagnosticId];

    public override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

    public override async Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
        if (root is null)
            return;

        var diagnostic = context.Diagnostics[0];
        var invocation = root.FindToken(diagnostic.Location.SourceSpan.Start)
                             .Parent?
                             .AncestorsAndSelf()
                             .OfType<InvocationExpressionSyntax>()
                             .FirstOrDefault(i => i.Span.Contains(diagnostic.Location.SourceSpan));

        if (invocation is null)
            return;

        var semanticModel = await context.Document.GetSemanticModelAsync(context.CancellationToken).ConfigureAwait(false);
        if (semanticModel is null)
            return;

        if (PreferToArrayExtensionAnalyzer.TryMatchSelectToTargetMethod(
                invocation,
                semanticModel,
                context.CancellationToken,
                out var collection,
                out var selector,
                out var methodName))
        {
            context.RegisterCodeFix(
                CodeAction.Create(
                    Title,
                    ct => ApplyFixAsync(context.Document, invocation, collection, selector, methodName, ct),
                    Title),
                diagnostic);
        }
    }

    static async Task<Document> ApplyFixAsync(
        Document document,
        InvocationExpressionSyntax oldInvocation,
        ExpressionSyntax collection,
        ExpressionSyntax selector,
        string methodName,
        CancellationToken ct)
    {
        var newInvocation = SyntaxFactory.InvocationExpression(
            SyntaxFactory.MemberAccessExpression(
                SyntaxKind.SimpleMemberAccessExpression,
                collection,
                SyntaxFactory.IdentifierName(methodName)))
            .WithArgumentList(SyntaxFactory.ArgumentList(
                SyntaxFactory.SingletonSeparatedList(SyntaxFactory.Argument(selector))))
            .WithAdditionalAnnotations(Formatter.Annotation);

        var root = await document.GetSyntaxRootAsync(ct).ConfigureAwait(false);
        return document.WithSyntaxRoot(root!.ReplaceNode(oldInvocation, newInvocation));
    }
}
