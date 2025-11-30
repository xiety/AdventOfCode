using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Advent.Analyzers;

[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(AddMapToAttributeCodeFix)), Shared]
public class AddMapToAttributeCodeFix : CodeFixProvider
{
    public sealed override ImmutableArray<string> FixableDiagnosticIds => [MissingMapToAttributeAnalyzer.DiagnosticId];
    public sealed override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

    public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        var diagnostic = context.Diagnostics.First();
        var typeName = diagnostic.Properties["TypeName"];

        var title = $"Add [MapTo<{typeName}>]";

        context.RegisterCodeFix(
            CodeAction.Create(title, c => AddAttributeAsync(context.Document, diagnostic, typeName!, c), title),
            diagnostic);
    }

    private async Task<Document> AddAttributeAsync(Document document, Diagnostic diagnostic, string typeName, CancellationToken c)
    {
        var root = await document.GetSyntaxRootAsync(c).ConfigureAwait(false);
        var node = root?.FindNode(diagnostic.Location.SourceSpan);

        var invocation = node?.FirstAncestorOrSelf<InvocationExpressionSyntax>();
        if (invocation?.Expression is not MemberAccessExpressionSyntax memberAccess)
            return document;
        if (memberAccess.Expression is not InvocationExpressionSyntax innerInvocation)
            return document;

        var semanticModel = await document.GetSemanticModelAsync(c).ConfigureAwait(false);

        if (semanticModel.GetSymbolInfo(innerInvocation).Symbol is not IMethodSymbol symbol || symbol.DeclaringSyntaxReferences.Length == 0)
            return document;

        var declarationRef = symbol.DeclaringSyntaxReferences[0];
        if (await declarationRef.GetSyntaxAsync(c) is not MethodDeclarationSyntax declarationSyntax)
            return document;

        var defDoc = document.Project.GetDocument(declarationSyntax.SyntaxTree);
        if (defDoc is null)
            return document;

        var name = SyntaxFactory.GenericName(SyntaxFactory.Identifier("MapTo"))
            .AddTypeArgumentListArguments(SyntaxFactory.ParseTypeName(typeName));

        var attribute = SyntaxFactory.Attribute(name);
        var attrList = SyntaxFactory.AttributeList(SyntaxFactory.SingletonSeparatedList(attribute));

        var newDeclaration = declarationSyntax.AddAttributeLists(attrList);

        var defRoot = await declarationSyntax.SyntaxTree.GetRootAsync(c);
        var newDefRoot = defRoot.ReplaceNode(declarationSyntax, newDeclaration);

        return defDoc.WithSyntaxRoot(newDefRoot);
    }
}
