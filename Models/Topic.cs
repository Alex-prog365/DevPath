using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevPath.Models
{
    public class Topic
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? ContentFile { get; set; }
        public int LessonId { get; set; }
        public bool IsCompleted { get; set; }
        public Lesson? Lesson { get; set; }
        public string? DisplayText
        {
            get 
            {
                return IsCompleted ? $"✓ {Title}" : Title;
            }
        }
        
    }
}
