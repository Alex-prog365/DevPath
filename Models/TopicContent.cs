using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevPath.Models
{
    using System.Collections.Generic;

    public class TopicContent
    {
        public List<ContentBlock> Blocks { get; set; } = new();
    }
}
