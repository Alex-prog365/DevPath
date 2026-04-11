using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevPath.Models
{
    public class RuleResult
    {
        public string RuleName { get; set; } = "";

        public bool Passed { get; set; }

        public string Message { get; set; } = "";
    }
}