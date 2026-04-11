using System;
using System.Text;
using System.Threading.Tasks;
using DevPath.Models;
using DevPath.Analysis;
using System.Collections.Generic;
using System.Linq;



namespace DevPath.Services
{
    public static class CodeValidator
    {
        public static EvaluationResult Validate(CodeAnalysisContext context)
        {
            var facts = FactCollector.Collect(context);

            if (facts.HasSyntaxErrors)
            {
                return new EvaluationResult
                {
                    IsPassed = false,
                    Message = "Syntax error in code",
                    ErrorCode = "SYNTAX_ERROR",
                    Facts = facts,
                    Details = facts.Diagnostics
                };
            }

            var ruleResults = new List<RuleResult>();

            foreach (var requiredFact in context.RequiredFacts)
            {
                var passed = CheckRequiredFact(requiredFact, facts);

                ruleResults.Add(new RuleResult
                {
                    RuleName = requiredFact,
                    Passed = passed,
                    Message = passed
                        ? $"{requiredFact}: OK"
                        : $"{requiredFact}: missing"
                });
            }

            var hasFailedRules = ruleResults.Any(r => !r.Passed);

            if (hasFailedRules)
            {
                return new EvaluationResult
                {
                    IsPassed = false,
                    Message = "Required code structure is missing",
                    ErrorCode = "RULE_FAILED",
                    Facts = facts,
                    RuleResults = ruleResults,
                    Details = ruleResults
                        .Where(r => !r.Passed)
                        .Select(r => r.Message)
                        .ToList()
                };
            }

            var normalizedUserCode = Normalize(context.UserCode);
            var normalizedExpectedCode = Normalize(context.ExpectedCode);

            if (normalizedUserCode == normalizedExpectedCode)
            {
                return new EvaluationResult
                {
                    IsPassed = true,
                    Message = "Correct",
                    ErrorCode = "OK",
                    Facts = facts,
                    RuleResults = ruleResults
                };
            }

            return new EvaluationResult
            {
                IsPassed = false,
                Message = "Try again",
                ErrorCode = "MISMATCH",
                Facts = facts,
                RuleResults = ruleResults
            };
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
                _ => true
            };
        }

        private static string Normalize(string code)
        {
            return (code ?? string.Empty)
                .Replace("\r\n", "\n")
                .Trim();
        }
    }
}