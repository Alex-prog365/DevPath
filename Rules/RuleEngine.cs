using DevPath.Models;

namespace DevPath.Rules
{
    public static class RuleEngine
    {
        public static List<RuleResult> Evaluate(CodeAnalysisContext context, CodeFacts facts)
        {
            var rules = RuleRegistry.GetRules();

            var results = new List<RuleResult>();

            foreach (var rule in rules)
            {
                if (!rule.AppliesTo(context))
                    continue;

                var result = rule.Evaluate(context, facts);
                results.Add(result);
            }

            return results;
        }
    }
}
