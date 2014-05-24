using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeRefactorings;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Formatting;
using Microsoft.CodeAnalysis.Text;

namespace RoslynSamples.AddMissingBracketsToIf
{
    [ExportCodeRefactoringProvider(CodeRefactoringProvider.RefactoringId, LanguageNames.CSharp)]
    internal class CodeRefactoringProvider : ICodeRefactoringProvider
    {
        public const string RefactoringId = "RoslynSamples.AddMissingBracketsToIf";

        public async Task<IEnumerable<CodeAction>> GetRefactoringsAsync(Document document, TextSpan textSpan, CancellationToken cancellationToken)
        {
            var root = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);

            // Find the node at the selection.
            var node = root.FindNode(textSpan);

            // Only offer a refactoring if the selected node is an if statement.
            var ifStatement = node as IfStatementSyntax;
            if (ifStatement == null)
            {
                return null;
            }

            // ... and its statement is not a block
            if (ifStatement.Statement != null && ifStatement.Statement.IsKind(SyntaxKind.Block))
            {
                return null;
            }

            // Create a code action that refactors the code
            var action = CodeAction.Create("Insert brackets", c => AddMissingBrackets(document, ifStatement, c));

            // Return this code action.
            return new[] { action };
        }

        private async Task<Solution> AddMissingBrackets(Document document, IfStatementSyntax ifStatement, CancellationToken cancellationToken)
        {
            // Find the syntax root
            var root = await document.GetSyntaxRootAsync(cancellationToken);

            // Create new if statement that corrects the old one
            var newIfStatement = ifStatement
                .WithStatement(SyntaxFactory.Block(ifStatement.Statement))
                // Add formatting to the output, that is, new lines for the brackets
                .WithAdditionalAnnotations(Formatter.Annotation);
            
            // Create a new root (notice that it is immutable)
            var newRoot = root.ReplaceNode(ifStatement, newIfStatement);

            // Return the new solution
            return document.WithSyntaxRoot(newRoot).Project.Solution;
        }
    }
}