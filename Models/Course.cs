using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevPath.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public int Progress { get; set; }

        public List<Lesson> Lessons { get; set; } = new();

        public string DisplayText => $"{Title} ({Progress}%)";
    }
}
