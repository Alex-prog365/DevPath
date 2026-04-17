using DevPath.Models;
using DevPath.Validation;

namespace DevPath.Rules
{
    public class VariableValueRule : IRule
    {
        public bool AppliesTo(CodeAnalysisContext context)
        {
            var profile = ValidationProfileRegistry.Get(context.ValidationProfile);

            return profile.CheckVariableValue &&
                   !string.IsNullOrWhiteSpace(context.RequiredVariableValue);
        }

        public RuleResult Evaluate(CodeAnalysisContext context, CodeFacts facts)
        {
            var hasRequiredValue =
                facts.VariableValues.ContainsValue(context.RequiredVariableValue);

            return new RuleResult
            {
                RuleName = "RequiredVariableValue",
                Passed = hasRequiredValue,
                Message = hasRequiredValue
                    ? $"Variable value '{context.RequiredVariableValue}' found"
                    : $"Variable value '{context.RequiredVariableValue}' is missing"
            };
        }
    }
}
