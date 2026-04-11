using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using DevPath.Models;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Linq;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DevPath.Analysis
{
    public static class FactCollector
    {
        public static CodeFacts Collect(CodeAnalysisContext context)
        {
            var facts = new CodeFacts();

            var code = context.UserCode ?? string.Empty;

            var syntaxTree = CSharpSyntaxTree.ParseText(code);
            var diagnostics = syntaxTree.GetDiagnostics().ToList();
            var root = syntaxTree.GetRoot();

            facts.HasSyntaxErrors = diagnostics.Any(d => d.Severity == DiagnosticSeverity.Error);

            foreach (var diagnostic in diagnostics)
            {
                facts.Diagnostics.Add(diagnostic.ToString());
            }

            facts.HasIfStatement = root.DescendantNodes().OfType<IfStatementSyntax>().Any();
            facts.HasForLoop = root.DescendantNodes().OfType<ForStatementSyntax>().Any();
            facts.HasWhileLoop = root.DescendantNodes().OfType<WhileStatementSyntax>().Any();

            var variableDeclarations = root.DescendantNodes().OfType<VariableDeclarationSyntax>().ToList();
            facts.HasVariableDeclaration = variableDeclarations.Any();

            foreach (var declaration in variableDeclarations)
            {
                foreach (var variable in declaration.Variables)
                {
                    facts.VariableNames.Add(variable.Identifier.Text);
                }
            }

            var methodDeclarations = root.DescendantNodes().OfType<MethodDeclarationSyntax>().ToList();

            foreach (var method in methodDeclarations)
            {
                facts.MethodNames.Add(method.Identifier.Text);
            }

            facts.HasConsoleReadLine = root
                .DescendantNodes()
                .OfType<InvocationExpressionSyntax>()
                .Any(invocation => invocation.ToString().Contains("Console.ReadLine"));

            facts.HasConsoleWriteLine = root
                .DescendantNodes()
                .OfType<InvocationExpressionSyntax>()
                .Any(invocation => invocation.ToString().Contains("Console.WriteLine"));

            return facts;
        }
    }
}