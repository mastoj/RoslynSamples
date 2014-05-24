using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace FailMissingIfBrackets
{
    // TODO: Consider implementing other interfaces that implement IDiagnosticAnalyzer instead of or in addition to ISymbolAnalyzer

    [DiagnosticAnalyzer]
    [ExportDiagnosticAnalyzer(DiagnosticId, LanguageNames.CSharp)]
    public class DiagnosticAnalyzer : ISyntaxNodeAnalyzer<SyntaxKind>
    {
        internal const string DiagnosticId = "FailMissingIfBrackets";
        internal const string Description = "The if statment is missing brackets";
        internal const string MessageFormat = "It would be nice if '{0}' has brackets";
        internal const string Category = "Syntax";

        internal static DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Description, MessageFormat, Category, DiagnosticSeverity.Warning);

        public ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
        {
            get { return ImmutableArray.Create(Rule); }
        }

        public ImmutableArray<SyntaxKind> SyntaxKindsOfInterest
        {
            get { return ImmutableArray.Create(SyntaxKind.IfStatement); }
        }

        public void AnalyzeNode(SyntaxNode node, SemanticModel semanticModel, Action<Diagnostic> addDiagnostic, CancellationToken cancellationToken)
        {
            var ifStatement = node as IfStatementSyntax;
            if (ifStatement != null &&
                ifStatement.Statement != null &&
                !ifStatement.Statement.IsKind(SyntaxKind.Block))
            {
                addDiagnostic(Diagnostic.Create(Rule, ifStatement.IfKeyword.GetLocation(), "if statments"));
            }
        }
    }
}
