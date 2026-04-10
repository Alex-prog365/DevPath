using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevPath.Services
{
    public static class CodeValidator
    {
        public static bool Validate(string userCode, string expectedCode)
        {
            var normalizedUserCode = Normalize(userCode);
            var normalizedExpectedCode = Normalize(expectedCode);

            return normalizedUserCode == normalizedExpectedCode;
        }

        private static string Normalize(string code)
        {
            return code .Replace("\r\n", "\n").Trim();
        }
    }
}