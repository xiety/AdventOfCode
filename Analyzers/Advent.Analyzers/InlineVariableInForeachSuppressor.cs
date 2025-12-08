using System.Collections.Immutable;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Advent.Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class InlineVariableInForeachSuppressor : DiagnosticSuppressor
{
    private static readonly SuppressionDescriptor suppression = new(
        id: "SPR1001",
        suppressedDiagnosticId: "RCS1124",
        justification: "Don't inline before foreach");

    public override ImmutableArray<SuppressionDescriptor> SupportedSuppressions =>
        [suppression];

    public override void ReportSuppressions(SuppressionAnalysisContext context)
    {
        foreach (var diagnostic in context.ReportedDiagnostics)
        {
            var root = diagnostic.Location.SourceTree?.GetRoot(context.CancellationToken);
            if (root == null)
                continue;

            var node = root.FindNode(diagnostic.Location.SourceSpan);

            VariableDeclaratorSyntax? variableDeclarator = null;

            if (node is LocalDeclarationStatementSyntax statement)
                variableDeclarator = statement.Declaration.Variables.FirstOrDefault();
            else
                variableDeclarator = node.FirstAncestorOrSelf<VariableDeclaratorSyntax>();

            if (variableDeclarator == null)
                continue;

            if (variableDeclarator.Parent is not VariableDeclarationSyntax variableDeclaration ||
                variableDeclaration.Parent is not LocalDeclarationStatementSyntax parentStatement ||
                parentStatement.Parent is not BlockSyntax block)
            {
                continue;
            }

            var declaredIndex = block.Statements.IndexOf(parentStatement);

            if (declaredIndex == -1 || declaredIndex + 1 >= block.Statements.Count)
                continue;

            var nextStatement = block.Statements[declaredIndex + 1];

            if (nextStatement is ForEachStatementSyntax foreachStatement)
            {
                if (foreachStatement.Expression is IdentifierNameSyntax identifierName &&
                    identifierName.Identifier.ValueText == variableDeclarator.Identifier.ValueText)
                {
                    context.ReportSuppression(Suppression.Create(suppression, diagnostic));
                }
            }
        }
    }
}
