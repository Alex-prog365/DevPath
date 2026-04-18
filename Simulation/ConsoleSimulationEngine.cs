using DevPath.Models;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DevPath.Simulation
{
    public static class ConsoleSimulationEngine
    {
        public static ConsoleSimulationResult Simulate(CodeAnalysisContext context)
        {
            var result = new ConsoleSimulationResult();

            if (context.UserCode.Contains("if") ||
                context.UserCode.Contains("for") ||
                context.UserCode.Contains("while"))
            {
                var output = RoslynExecutionEngine.Execute(context.UserCode, context.FakeInput ?? "");

                result.HasConsoleInteraction = true;

                foreach (var line in output.Split('\n'))
                {
                    if (!string.IsNullOrWhiteSpace(line))
                        result.OutputLines.Add(line.Trim());
                }

                return result;
            }

            var code = context.UserCode ?? string.Empty;
            var syntaxTree = CSharpSyntaxTree.ParseText(code);
            var root = syntaxTree.GetRoot();

            var variableValues = new Dictionary<string, string>();
            var fakeInputQueue = BuildFakeInputQueue(context);

            CollectVariableDeclarations(root, variableValues);
            ApplyReadLineAssignments(root, variableValues, fakeInputQueue, result);
            ApplyNormalAssignments(root, variableValues);

            var invocations = root.DescendantNodes().OfType<InvocationExpressionSyntax>().ToList();

            foreach (var invocation in invocations)
            {
                if (!IsConsoleWriteLine(invocation))
                    continue;

                var line = EvaluateWriteLineArgument(invocation, variableValues);

                result.HasConsoleInteraction = true;
                result.OutputLines.Add(line);
            }

            return result;
        }

        private static Queue<string> BuildFakeInputQueue(CodeAnalysisContext context)
        {
            var queue = new Queue<string>();

            if (!string.IsNullOrWhiteSpace(context.FakeInput))
            {
                var lines = context.FakeInput
                    .Replace("\r\n", "\n")
                    .Split('\n')
                    .Where(line => !string.IsNullOrWhiteSpace(line));

                foreach (var line in lines)
                {
                    queue.Enqueue(line);
                }
            }

            return queue;
        }

        private static void CollectVariableDeclarations(
            SyntaxNode root,
            Dictionary<string, string> variableValues)
        {
            var variableDeclarations = root.DescendantNodes().OfType<VariableDeclarationSyntax>().ToList();

            foreach (var declaration in variableDeclarations)
            {
                foreach (var variable in declaration.Variables)
                {
                    var variableName = variable.Identifier.Text;
                    var initializer = variable.Initializer?.Value;

                    if (initializer == null)
                    {
                        if (!variableValues.ContainsKey(variableName))
                        {
                            variableValues.Add(variableName, "");
                        }

                        continue;
                    }

                    if (initializer is InvocationExpressionSyntax invocation &&
                        IsConsoleReadLine(invocation))
                    {
                        continue;
                    }

                    var valueText = initializer.ToString();

                    if (!variableValues.ContainsKey(variableName))
                    {
                        variableValues.Add(variableName, valueText);
                    }
                    else
                    {
                        variableValues[variableName] = valueText;
                    }
                }
            }
        }

        private static void ApplyReadLineAssignments(
            SyntaxNode root,
            Dictionary<string, string> variableValues,
            Queue<string> fakeInputQueue,
            ConsoleSimulationResult result)
        {
            var variableDeclarations = root.DescendantNodes().OfType<VariableDeclarationSyntax>().ToList();

            foreach (var declaration in variableDeclarations)
            {
                foreach (var variable in declaration.Variables)
                {
                    var initializer = variable.Initializer?.Value;

                    if (initializer is InvocationExpressionSyntax invocation &&
                        IsConsoleReadLine(invocation))
                    {
                        var variableName = variable.Identifier.Text;
                        var inputValue = fakeInputQueue.Count > 0 ? fakeInputQueue.Dequeue() : "";

                        if (!variableValues.ContainsKey(variableName))
                        {
                            variableValues.Add(variableName, inputValue);
                        }
                        else
                        {
                            variableValues[variableName] = inputValue;
                        }

                        result.HasConsoleInteraction = true;
                        result.InputValues.Add(inputValue);
                    }
                }
            }

            var assignments = root.DescendantNodes().OfType<AssignmentExpressionSyntax>().ToList();

            foreach (var assignment in assignments)
            {
                if (assignment.Left is IdentifierNameSyntax identifier &&
                    assignment.Right is InvocationExpressionSyntax invocation &&
                    IsConsoleReadLine(invocation))
                {
                    var variableName = identifier.Identifier.Text;
                    var inputValue = fakeInputQueue.Count > 0 ? fakeInputQueue.Dequeue() : "";

                    if (!variableValues.ContainsKey(variableName))
                    {
                        variableValues.Add(variableName, inputValue);
                    }
                    else
                    {
                        variableValues[variableName] = inputValue;
                    }

                    result.HasConsoleInteraction = true;
                    result.InputValues.Add(inputValue);
                }
            }
        }

        private static void ApplyNormalAssignments(
            SyntaxNode root,
            Dictionary<string, string> variableValues)
        {
            var assignments = root.DescendantNodes().OfType<AssignmentExpressionSyntax>().ToList();

            foreach (var assignment in assignments)
            {
                if (assignment.Left is not IdentifierNameSyntax identifier)
                    continue;

                if (assignment.Right is InvocationExpressionSyntax invocation &&
                    IsConsoleReadLine(invocation))
                    continue;

                var variableName = identifier.Identifier.Text;
                var valueText = assignment.Right.ToString();

                if (!variableValues.ContainsKey(variableName))
                {
                    variableValues.Add(variableName, valueText);
                }
                else
                {
                    variableValues[variableName] = valueText;
                }
            }
        }

        private static bool IsConsoleWriteLine(InvocationExpressionSyntax invocation)
        {
            if (invocation.Expression is MemberAccessExpressionSyntax memberAccess)
            {
                return memberAccess.Expression.ToString() == "Console"
                    && memberAccess.Name.Identifier.Text == "WriteLine";
            }

            return false;
        }

        private static bool IsConsoleReadLine(InvocationExpressionSyntax invocation)
        {
            if (invocation.Expression is MemberAccessExpressionSyntax memberAccess)
            {
                return memberAccess.Expression.ToString() == "Console"
                    && memberAccess.Name.Identifier.Text == "ReadLine";
            }

            return false;
        }

        private static string EvaluateWriteLineArgument(
            InvocationExpressionSyntax invocation,
            Dictionary<string, string> variableValues)
        {
            var argument = invocation.ArgumentList.Arguments.FirstOrDefault();

            if (argument == null)
                return "";

            var expression = argument.Expression;

            if (expression is LiteralExpressionSyntax literal)
            {
                return NormalizeLiteralText(literal.Token.ValueText, literal.ToString());
            }

            if (expression is IdentifierNameSyntax identifier)
            {
                var variableName = identifier.Identifier.Text;

                if (variableValues.TryGetValue(variableName, out var value))
                {
                    return NormalizeStoredValue(value);
                }

                return variableName;
            }

            return NormalizeStoredValue(expression.ToString());
        }

        private static string NormalizeStoredValue(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return "";

            if (value.StartsWith("\"") && value.EndsWith("\"") && value.Length >= 2)
            {
                return value.Substring(1, value.Length - 2);
            }

            return value;
        }

        private static string NormalizeLiteralText(string valueText, string rawText)
        {
            if (rawText.StartsWith("\"") && rawText.EndsWith("\""))
            {
                return valueText;
            }

            return rawText;
        }
    }
}
