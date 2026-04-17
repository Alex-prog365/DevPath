using DevPath.Models;

namespace DevPath.Rules
{
    public interface IRule
    {
        bool AppliesTo(CodeAnalysisContext context);
        RuleResult Evaluate(CodeAnalysisContext context, CodeFacts facts);
    }
}
