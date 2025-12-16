using System.Collections.Immutable;
using System.Composition;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Advent.Analyzers;

[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(RegexMapCodeFixProvider)), Shared]
public class RegexMapCodeFixProvider : CodeFixProvider
{
    public sealed override ImmutableArray<string> FixableDiagnosticIds => [RegexMapAnalyzer.DiagnosticId];
    public sealed override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

    public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
        var diagnostic = context.Diagnostics.First();
        var node = root?.FindNode(diagnostic.Location.SourceSpan);

        var invocation = node?.FirstAncestorOrSelf<InvocationExpressionSyntax>();
        if (invocation == null)
            return;

        if (invocation.Expression is not MemberAccessExpressionSyntax memberAccess)
            return;
        if (memberAccess.Expression is not InvocationExpressionSyntax innerInvocation)
            return;
        if (innerInvocation.Expression is not MemberAccessExpressionSyntax innerMemberAccess)
            return;

        var newMethodName = $"{memberAccess.Name.Identifier.Text}{innerMemberAccess.Name.Identifier.Text}";
        var title = $"Use '{newMethodName}'";

        context.RegisterCodeFix(
            CodeAction.Create(title, c => ReplaceCallAsync(context.Document, invocation, innerMemberAccess, newMethodName, c), title),
            diagnostic);
    }

    private async Task<Document> ReplaceCallAsync(Document document, InvocationExpressionSyntax fullInvocation, MemberAccessExpressionSyntax staticClassAccess, string newMethodName, CancellationToken cancellationToken)
    {
        var root = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
        var classExpression = staticClassAccess.Expression;

        var newMemberAccess = SyntaxFactory.MemberAccessExpression(
            SyntaxKind.SimpleMemberAccessExpression,
            classExpression,
            SyntaxFactory.IdentifierName(newMethodName)
        );

        var newInvocation = SyntaxFactory.InvocationExpression(newMemberAccess, fullInvocation.ArgumentList)
            .WithLeadingTrivia(fullInvocation.GetLeadingTrivia())
            .WithTrailingTrivia(fullInvocation.GetTrailingTrivia());

        var newRoot = root!.ReplaceNode(fullInvocation, newInvocation);
        return document.WithSyntaxRoot(newRoot);
    }
}
