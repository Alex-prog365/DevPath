using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections.Generic;

using System.Collections.Generic;

using System.Collections.Generic;

namespace DevPath.Models
{
    public class CodeFacts
    {
        public bool HasSyntaxErrors { get; set; }

        public List<string> Diagnostics { get; set; } = new();

        public bool HasVariableDeclaration { get; set; }

        public List<string> VariableNames { get; set; } = new();

        public Dictionary<string, string> VariableTypes { get; set; } = new();

        public Dictionary<string, string> VariableValues { get; set; } = new();

        public bool HasConsoleReadLine { get; set; }

        public bool HasConsoleWriteLine { get; set; }

        public bool HasIfStatement { get; set; }

        public bool HasForLoop { get; set; }

        public bool HasWhileLoop { get; set; }

        public List<string> MethodNames { get; set; } = new();

        public List<string> InvokedMethodNames { get; set; } = new();

        public bool UsesVarKeyword { get; set; }
    }
}