using DevPath.Models;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq;
using System.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                facts.Diagnostics.Add(diagnostic.GetMessage(CultureInfo.InvariantCulture));
            }

            facts.HasIfStatement = root.DescendantNodes().OfType<IfStatementSyntax>().Any();
            facts.HasForLoop = root.DescendantNodes().OfType<ForStatementSyntax>().Any();
            facts.HasWhileLoop = root.DescendantNodes().OfType<WhileStatementSyntax>().Any();

            

            var variableDeclarations = root.DescendantNodes().OfType<VariableDeclarationSyntax>().ToList();
            facts.HasVariableDeclaration = variableDeclarations.Any();

            foreach (var declaration in variableDeclarations)
            {
                var typeName = declaration.Type.ToString();

                if (typeName == "var")
                {
                    facts.UsesVarKeyword = true;
                }

                foreach (var variable in declaration.Variables)
                {
                    var variableName = variable.Identifier.Text;

                    facts.VariableNames.Add(variableName);

                    if (!facts.VariableTypes.ContainsKey(variableName))
                    {
                        facts.VariableTypes.Add(variableName, typeName);
                    }

                    var valueText = variable.Initializer?.Value?.ToString() ?? "";

                    if (!facts.VariableValues.ContainsKey(variableName))
                    {
                        facts.VariableValues.Add(variableName, valueText);
                    }
                }
            }
                       
            var assignments = root.DescendantNodes().OfType<AssignmentExpressionSyntax>().ToList();

            foreach (var assignment in assignments)
            {
                if (assignment.Left is IdentifierNameSyntax identifier)
                {
                    var variableName = identifier.Identifier.Text;
                    var valueText = assignment.Right.ToString();

                    if (facts.VariableValues.ContainsKey(variableName))
                    {
                        facts.VariableValues[variableName] = valueText;
                    }
                    else
                    {
                        facts.VariableValues.Add(variableName, valueText);
                    }
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