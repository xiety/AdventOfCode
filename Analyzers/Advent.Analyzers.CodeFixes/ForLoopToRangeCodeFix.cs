using System.Collections.Immutable;
using System.Composition;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Simplification;

namespace Advent.Analyzers;

[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(ForLoopToRangeCodeFix)), Shared]
public sealed class ForLoopToRangeCodeFix : CodeFixProvider
{
    const string Title = "Convert to foreach with range";

    public override ImmutableArray<string> FixableDiagnosticIds
        => [ForLoopToRangeAnalyzer.DiagnosticId];

    public override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

    public override async Task RegisterCodeFixesAsync(CodeFixContext ctx)
    {
        var root = await ctx.Document.GetSyntaxRootAsync(ctx.CancellationToken).ConfigureAwait(false);
        if (root is null)
            return;

        var diag = ctx.Diagnostics[0];
        var token = root.FindToken(diag.Location.SourceSpan.Start);
        var forStmt = token.Parent?.AncestorsAndSelf().OfType<ForStatementSyntax>().FirstOrDefault();
        if (forStmt is null)
            return;

        ctx.RegisterCodeFix(CodeAction.Create(Title, ct => ConvertAsync(ctx.Document, forStmt, ct), Title), diag);
    }

    async Task<Document> ConvertAsync(Document doc, ForStatementSyntax f, CancellationToken ct)
    {
        var root = await doc.GetSyntaxRootAsync(ct).ConfigureAwait(false);
        if (root is null)
            return doc;

        var id = f.Declaration!.Variables[0].Identifier.ValueText;
        var low = f.Declaration.Variables[0].Initializer!.Value;
        var bin = (BinaryExpressionSyntax)f.Condition!;
        var up = bin.Right!;

        var sem = await doc.GetSemanticModelAsync(ct);
        var cv = sem!.GetConstantValue(low, ct);
        var isZeroStart = cv is { HasValue: true, Value: 0 or 0L or 0F or 0D };

        if (isZeroStart)
        {
            var fe = SyntaxFactory.ForEachStatement(
                SyntaxFactory.IdentifierName("var"),
                id,
                up,
                f.Statement)
                .WithLeadingTrivia(f.GetLeadingTrivia());

            var newRoot = root.ReplaceNode(f, fe);
            return await Simplifier.ReduceAsync(doc.WithSyntaxRoot(newRoot), optionSet: null, cancellationToken: ct);
        }

        if (bin.Kind() == SyntaxKind.LessThanOrEqualExpression)
            up = AddOne(up);

        low = WrapAndAnnotate(low);
        up = WrapAndAnnotate(up);

        var range = SyntaxFactory.RangeExpression(low, up);
        var fe2 = SyntaxFactory.ForEachStatement(
            SyntaxFactory.IdentifierName("var"), id, range, f.Statement)
            .WithLeadingTrivia(f.GetLeadingTrivia());

        var newRoot2 = root.ReplaceNode(f, fe2);
        return await Simplifier.ReduceAsync(doc.WithSyntaxRoot(newRoot2), optionSet: null, cancellationToken: ct);
    }

    static ExpressionSyntax AddOne(ExpressionSyntax e)
    {
        if (e is LiteralExpressionSyntax lit)
        {
            var v = int.Parse(lit.Token.ValueText) + 1;
            return SyntaxFactory.LiteralExpression(SyntaxKind.NumericLiteralExpression, SyntaxFactory.Literal(v));
        }

        if (e is BinaryExpressionSyntax sub && sub.Kind() == SyntaxKind.SubtractExpression &&
            sub.Right is LiteralExpressionSyntax r)
        {
            var k = int.Parse(r.Token.ValueText);
            if (k == 1)
                return sub.Left;
            var newLit = SyntaxFactory.LiteralExpression(SyntaxKind.NumericLiteralExpression, SyntaxFactory.Literal(k - 1));
            return SyntaxFactory.ParenthesizedExpression(
                SyntaxFactory.BinaryExpression(SyntaxKind.SubtractExpression, sub.Left, newLit));
        }

        return SyntaxFactory.ParenthesizedExpression(
            SyntaxFactory.BinaryExpression(SyntaxKind.AddExpression, e,
                SyntaxFactory.LiteralExpression(SyntaxKind.NumericLiteralExpression, SyntaxFactory.Literal(1))));
    }

    static ExpressionSyntax WrapAndAnnotate(ExpressionSyntax e)
    {
        e = e switch
        {
            LiteralExpressionSyntax or IdentifierNameSyntax => e,
            _ => SyntaxFactory.ParenthesizedExpression(e)
        };
        return e.WithAdditionalAnnotations(Simplifier.Annotation);
    }
}
