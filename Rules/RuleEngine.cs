using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections.Generic;
using System.Linq;
using DevPath.Models;

namespace DevPath.Rules
{
    public static class RuleEngine
    {
        public static List<RuleResult> Evaluate(CodeAnalysisContext context, CodeFacts facts)
        {
            var ruleResults = new List<RuleResult>();

            foreach (var requiredFact in context.RequiredFacts)
            {
                var passed = CheckRequiredFact(requiredFact, facts);

                ruleResults.Add(new RuleResult
                {
                    RuleName = requiredFact,
                    Passed = passed,
                    Message = GetRuleMessage(requiredFact, passed)
                });
            }

            var hasRequiredVariable = true;

            if (!string.IsNullOrWhiteSpace(context.RequiredVariableName))
            {
                hasRequiredVariable = facts.VariableNames.Contains(context.RequiredVariableName);

                ruleResults.Add(new RuleResult
                {
                    RuleName = "RequiredVariableName",
                    Passed = hasRequiredVariable,
                    Message = hasRequiredVariable
                        ? $"Variable '{context.RequiredVariableName}' found"
                        : $"Variable '{context.RequiredVariableName}' is missing"
                });
            }

            if (hasRequiredVariable &&
                !string.IsNullOrWhiteSpace(context.RequiredVariableName) &&
                !string.IsNullOrWhiteSpace(context.RequiredVariableType))
            {
                var hasCorrectType =
                    facts.VariableTypes.TryGetValue(context.RequiredVariableName, out var actualType) &&
                    actualType == context.RequiredVariableType;

                ruleResults.Add(new RuleResult
                {
                    RuleName = "RequiredVariableType",
                    Passed = hasCorrectType,
                    Message = hasCorrectType
                        ? $"Variable '{context.RequiredVariableName}' has correct type '{context.RequiredVariableType}'"
                        : $"Variable '{context.RequiredVariableName}' must have type '{context.RequiredVariableType}'"
                });
            }

            if (hasRequiredVariable &&
                !string.IsNullOrWhiteSpace(context.RequiredVariableName) &&
                !string.IsNullOrWhiteSpace(context.RequiredVariableValue))
            {
                var hasCorrectValue =
                    facts.VariableValues.TryGetValue(context.RequiredVariableName, out var actualValue) &&
                    Normalize(actualValue) == Normalize(context.RequiredVariableValue);

                ruleResults.Add(new RuleResult
                {
                    RuleName = "RequiredVariableValue",
                    Passed = hasCorrectValue,
                    Message = hasCorrectValue
                        ? $"Variable '{context.RequiredVariableName}' has correct value"
                        : $"Variable '{context.RequiredVariableName}' must have value {context.RequiredVariableValue}"
                });
            }

            return ruleResults;
        }

        private static bool CheckRequiredFact(string factName, CodeFacts facts)
        {
            return factName switch
            {
                "HasIfStatement" => facts.HasIfStatement,
                "HasForLoop" => facts.HasForLoop,
                "HasWhileLoop" => facts.HasWhileLoop,
                "HasVariableDeclaration" => facts.HasVariableDeclaration,
                "HasConsoleReadLine" => facts.HasConsoleReadLine,
                "HasConsoleWriteLine" => facts.HasConsoleWriteLine,
                "UsesVarKeyword" => facts.UsesVarKeyword,
                _ => true
            };
        }

        private static string Normalize(string text)
        {
            return (text ?? string.Empty)
                .Replace("\r\n", "\n")
                .Trim();
        }

        private static string GetRuleMessage(string ruleName, bool passed)
        {
            return ruleName switch
            {
                "HasVariableDeclaration" => passed
                    ? "Variable declaration found"
                    : "You need to declare a variable.",

                "UsesVarKeyword" => passed
                    ? "The var keyword is used correctly."
                    : "Use 'var' in this task.",

                "HasIfStatement" => passed
                    ? "If statement found"
                    : "You need to use 'if' in this task.",

                "HasForLoop" => passed
                    ? "For loop found"
                    : "You need to use 'for' in this task.",

                "HasWhileLoop" => passed
                    ? "While loop found"
                    : "You need to use 'while' in this task.",

                "HasConsoleReadLine" => passed
                    ? "Input reading found"
                    : "You need to use Console.ReadLine().",

                "HasConsoleWriteLine" => passed
                    ? "Output found"
                    : "You need to use Console.WriteLine().",

                _ => passed
                    ? "Rule passed"
                    : "Required code structure is missing."
            };
        }
    }
}
