using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace DevPath.Models
{
    public class CodeAnalysisContext
    {
        public string UserCode { get; set; } = "";

        public string ExpectedCode { get; set; } = "";

        public string TaskType { get; set; } = "";

        public string TopicTitle { get; set; } = "";

        public List<string> RequiredFacts { get; set; } = new();
    }
}