using System;
using System.Text;
using System.Threading.Tasks;
using DevPath.Models;
using DevPath.Analysis;
using System.Collections.Generic;
using System.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Linq;
using DevPath.Rules;
using DevPath.Simulation;

namespace DevPath.Services
{
    public static class CodeValidator
    {
        public static EvaluationResult Validate(CodeAnalysisContext context)
        {
            var facts = FactCollector.Collect(context);
            var simulationResult = ConsoleSimulationEngine.Simulate(context);
            var executionOutput = CodeExecutionService.Execute(context.UserCode, context.FakeInput);

            if (facts.HasSyntaxErrors)
            {
                return new EvaluationResult
                {
                    IsPassed = false,
                    Message = "Syntax error in code",
                    ErrorCode = "SYNTAX_ERROR",
                    Facts = facts,
                    Details = facts.Diagnostics,
                    ConsoleSimulation = simulationResult
                };
            }

            var ruleResults = RuleEngine.Evaluate(context, facts);
            var failedRules = ruleResults.Where(r => !r.Passed).ToList();

            if (failedRules.Any())
            {
                var messages = failedRules.Select(r => r.Message).ToList();

                return new EvaluationResult
                {
                    IsPassed = false,
                    Message = messages.First(),
                    ErrorCode = "RULE_FAILED",
                    Facts = facts,
                    RuleResults = ruleResults,
                    Details = messages,
                    ConsoleSimulation = simulationResult
                };
            }

            if (!string.IsNullOrWhiteSpace(context.ExpectedOutput))
            {
                var actualOutput = string.Join("\n", executionOutput).Trim();
                var expectedOutput = context.ExpectedOutput.Trim();

                if (actualOutput == expectedOutput)
                {
                    return new EvaluationResult
                    {
                        IsPassed = true,
                        Message = "Correct",
                        ErrorCode = "OK",
                        Facts = facts,
                        RuleResults = ruleResults,
                        ConsoleSimulation = simulationResult,
                        ExecutionOutput = executionOutput
                    };
                }

                return new EvaluationResult
                {
                    IsPassed = false,
                    Message = "Output is incorrect",
                    ErrorCode = "OUTPUT_MISMATCH",
                    Facts = facts,
                    RuleResults = ruleResults,
                    Details = new List<string>
                    {
                        "Expected output:",
                        expectedOutput,
                        "Actual output:",
                        actualOutput
                    },
                    ConsoleSimulation = simulationResult,
                    ExecutionOutput = executionOutput
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
                    RuleResults = ruleResults,
                    ConsoleSimulation = simulationResult
                };
            }

            return new EvaluationResult
            {
                IsPassed = false,
                Message = "Try again",
                ErrorCode = "MISMATCH",
                Facts = facts,
                RuleResults = ruleResults,
                ConsoleSimulation = simulationResult
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