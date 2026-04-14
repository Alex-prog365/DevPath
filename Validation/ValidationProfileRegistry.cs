using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections.Generic;

namespace DevPath.Validation
{
    public static class ValidationProfileRegistry
    {
        public static ValidationProfileDefinition Get(string profileName)
        {
            return profileName switch
            {
                "variable_basic" => new ValidationProfileDefinition
                {
                    RequiredFacts = new List<string> { "HasVariableDeclaration" },
                    CheckVariableName = true,
                    CheckVariableType = true,
                    CheckVariableValue = true
                },

                "variable_change" => new ValidationProfileDefinition
                {
                    RequiredFacts = new List<string> { "HasVariableDeclaration" },
                    CheckVariableName = true,
                    CheckVariableType = true,
                    CheckVariableValue = true
                },

                "variable_var" => new ValidationProfileDefinition
                {
                    RequiredFacts = new List<string> { "HasVariableDeclaration", "UsesVarKeyword" },
                    CheckVariableName = true,
                    CheckVariableType = false,
                    CheckVariableValue = true
                },

                "variable_const" => new ValidationProfileDefinition
                {
                    RequiredFacts = new List<string> { "HasVariableDeclaration" },
                    CheckVariableName = true,
                    CheckVariableType = true,
                    CheckVariableValue = true
                },

                "input_output_write" => new ValidationProfileDefinition
                {
                    RequiredFacts = new List<string> { "HasConsoleWriteLine" },
                    CheckVariableName = false,
                    CheckVariableType = false,
                    CheckVariableValue = false
                },

                "input_output_read" => new ValidationProfileDefinition
                {
                    RequiredFacts = new List<string> { "HasConsoleReadLine", "HasVariableDeclaration" },
                    CheckVariableName = true,
                    CheckVariableType = false,
                    CheckVariableValue = false
                },

                "input_output_read_write" => new ValidationProfileDefinition
                {
                    RequiredFacts = new List<string> { "HasConsoleReadLine", "HasConsoleWriteLine", "HasVariableDeclaration" },
                    CheckVariableName = true,
                    CheckVariableType = false,
                    CheckVariableValue = false
                },

                _ => new ValidationProfileDefinition()
            };
        }
    }
}