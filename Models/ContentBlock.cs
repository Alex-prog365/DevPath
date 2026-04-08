using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevPath.Models
{
    public class ContentBlock
    {
        public string Type { get; set; } = "";
        public string Content { get; set; } = "";
        public string ExpectedAnswer { get; set; } = "";
    }
}
