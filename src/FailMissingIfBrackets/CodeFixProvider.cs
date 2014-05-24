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
            // Get the syntax root
            var root = await document.GetSyntaxRootAsync(cancellationToken);
            // Find the token that triggered the fix
            var token = root.FindToken(span.Start);
            if (token.IsKind(SyntaxKind.IfKeyword))
            {
                // Find the statement you want to change
                var ifStatment = (IfStatementSyntax)token.Parent;
                
                // Create replacement statement
                var newIfStatement = ifStatment.WithStatement(SyntaxFactory.Block(ifStatment.Statement))
                    .WithAdditionalAnnotations(Formatter.Annotation);
                // New root based on the old one
                var newRoot = root.ReplaceNode(ifStatment, newIfStatement);
                return new[] {CodeAction.Create("Add braces", document.WithSyntaxRoot(newRoot))};
            }
            return null;
        }
    }
}