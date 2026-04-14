using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace DevPath.Models
{
    public class ContentBlock
    {
        public string Type { get; set; } = "";

        public string Content { get; set; } = "";

        public string Code { get; set; } = "";

        public string ExpectedAnswer { get; set; } = "";
               
        public string RequiredVariableName { get; set; } = "";

        public string RequiredVariableType { get; set; } = "";

        public string RequiredVariableValue { get; set; } = "";

        public string ValidationProfile { get; set; } = "";

        public string FakeInput { get; set; } = "";
    }
}