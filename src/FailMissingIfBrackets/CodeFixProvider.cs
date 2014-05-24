using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Formatting;
using Microsoft.CodeAnalysis.Rename;
using Microsoft.CodeAnalysis.Text;

namespace FailMissingIfBrackets
{
    [ExportCodeFixProvider(DiagnosticAnalyzer.DiagnosticId, LanguageNames.CSharp)]
    internal class CodeFixProvider : ICodeFixProvider
    {
        public IEnumerable<string> GetFixableDiagnosticIds()
        {
            return new[] { DiagnosticAnalyzer.DiagnosticId };
        }

        public async Task<IEnumerable<CodeAction>> GetFixesAsync(Document document, TextSpan span, IEnumerable<Diagnostic> diagnostics, CancellationToken cancellationToken)
        {
            var root = await document.GetSyntaxRootAsync(cancellationToken);
            var token = root.FindToken(span.Start);
            if (token.IsKind(SyntaxKind.IfKeyword))
            {
                var ifStatment = (IfStatementSyntax)token.Parent;
                var newIfStatement = ifStatment.WithStatement(SyntaxFactory.Block(ifStatment.Statement))
                    .WithAdditionalAnnotations(Formatter.Annotation);
                var newRoot = root.ReplaceNode(ifStatment, newIfStatement);
                return new[] {CodeAction.Create("Add braces", document.WithSyntaxRoot(newRoot))};
            }
            return null;
        }
    }
}