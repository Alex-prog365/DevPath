using DevPath.Models;
using DevPath.Validation;

namespace DevPath.Rules
{
    public class VariableNameRule : IRule
    {
        public bool AppliesTo(CodeAnalysisContext context)
        {
            var profile = ValidationProfileRegistry.Get(context.ValidationProfile);

            return profile.CheckVariableName &&
                   !string.IsNullOrWhiteSpace(context.RequiredVariableName);
        }

        public RuleResult Evaluate(CodeAnalysisContext context, CodeFacts facts)
        {
            var hasRequiredVariable =
                facts.VariableNames.Contains(context.RequiredVariableName);

            return new RuleResult
            {
                RuleName = "RequiredVariableName",
                Passed = hasRequiredVariable,
                Message = hasRequiredVariable
                    ? $"Variable '{context.RequiredVariableName}' found"
                    : $"Variable '{context.RequiredVariableName}' is missing"
            };
        }
    }
}