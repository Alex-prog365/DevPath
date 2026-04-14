using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using DevPath.Simulation;
using System.Collections.Generic;

namespace DevPath.Models
{
    public class EvaluationResult
    {
        public bool IsPassed { get; set; }

        public string Message { get; set; } = "";

        public string ErrorCode { get; set; } = "";

        public List<string> Details { get; set; } = new();

        public CodeFacts Facts { get; set; } = new();

        public List<RuleResult> RuleResults { get; set; } = new();

        public ConsoleSimulationResult ConsoleSimulation { get; set; } = new();
    }
}