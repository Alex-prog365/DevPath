using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections.Generic;

namespace DevPath.Rules
{
    public static class RuleRegistry
    {
        public static List<IRule> GetRules()
        {
            return new List<IRule>
            {
                new RequiredFactRule(),
                new VariableNameRule(),
                new VariableTypeRule(),
                new VariableValueRule()
            };
        }
    }
}
