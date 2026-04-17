using DevPath.Models;
using DevPath.Validation;

namespace DevPath.Rules
{
    public class RequiredFactRule : IRule
    {
        public bool AppliesTo(CodeAnalysisContext context)
        {
            var profile = ValidationProfileRegistry.Get(context.ValidationProfile);
            return profile.RequiredFacts.Count > 0;
        }

        public RuleResult Evaluate(CodeAnalysisContext context, CodeFacts facts)
        {
            var profile = ValidationProfileRegistry.Get(context.ValidationProfile);

            var requiredFacts = profile.RequiredFacts;

            foreach (var requiredFact in requiredFacts)
            {
                var passed = CheckRequiredFact(requiredFact, facts);

                if (!passed)
                {
                    return new RuleResult
                    {
                        RuleName = requiredFact,
                        Passed = false,
                        Message = $"Required fact '{requiredFact}' is missing"
                    };
                }
            }

            return new RuleResult
            {
                RuleName = "RequiredFacts",
                Passed = true
            };
        }

        private bool CheckRequiredFact(string requiredFact, CodeFacts facts)
        {
            return requiredFact switch
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
    }
}