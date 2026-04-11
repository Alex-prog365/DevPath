using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevPath.Models
{
    public class ValidationResult
    {
        public bool IsCorrect { get; set; }

        public string Message { get; set; } = "";

        public string ErrorCode { get; set; } = "";
    }
}