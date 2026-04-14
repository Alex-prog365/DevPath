using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections.Generic;

namespace DevPath.Simulation
{
    public class ConsoleSimulationResult
    {
        public bool HasConsoleInteraction { get; set; }

        public List<string> OutputLines { get; set; } = new();

        public List<string> InputValues { get; set; } = new();
    }
}