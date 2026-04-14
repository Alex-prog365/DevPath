using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections.Generic;

namespace DevPath.Validation
{
    public class ValidationProfileDefinition
    {
        public List<string> RequiredFacts { get; set; } = new();

        public bool CheckVariableName { get; set; }

        public bool CheckVariableType { get; set; }

        public bool CheckVariableValue { get; set; }
    }
}