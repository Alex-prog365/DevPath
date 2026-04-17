using DevPath.Models;
using DevPath.Validation;

namespace DevPath.Rules
{
    public class VariableTypeRule : IRule
    {
        public bool AppliesTo(CodeAnalysisContext context)
        {
            var profile = ValidationProfileRegistry.Get(context.ValidationProfile);

            return profile.CheckVariableType &&
                   !string.IsNullOrWhiteSpace(context.RequiredVariableType);
        }

        public RuleResult Evaluate(CodeAnalysisContext context, CodeFacts facts)
        {
            var hasRequiredType =
                facts.VariableTypes.ContainsValue(context.RequiredVariableType);

            return new RuleResult
            {
                RuleName = "RequiredVariableType",
                Passed = hasRequiredType,
                Message = hasRequiredType
                    ? $"Variable type '{context.RequiredVariableType}' found"
                    : $"Variable type '{context.RequiredVariableType}' is missing"
            };
        }
    }
}
