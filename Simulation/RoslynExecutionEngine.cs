using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace DevPath.Simulation
{
    public static class RoslynExecutionEngine
    {
        public static string Execute(string userCode, string fakeInput)
        {
            var fullCode = WrapCode(userCode);

            var syntaxTree = CSharpSyntaxTree.ParseText(fullCode);

            var compilation = CSharpCompilation.Create(
                "UserProgram",
                new[] { syntaxTree },

                ((string)AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES")!)
                    .Split(Path.PathSeparator)
                    .Select(path => MetadataReference.CreateFromFile(path)),
                new CSharpCompilationOptions(OutputKind.ConsoleApplication)
            );

            using var ms = new MemoryStream();

            var result = compilation.Emit(ms);

            if (!result.Success)
            {
                var firstError = result.Diagnostics
                    .FirstOrDefault(d => d.Severity == DiagnosticSeverity.Error);

                if (firstError != null)
                    return "Compilation error: " + firstError.GetMessage();

                return "Compilation error";
            }

            ms.Seek(0, SeekOrigin.Begin);

            var assembly = Assembly.Load(ms.ToArray());

            var entry = assembly.EntryPoint;

            if (entry == null)
                return "Runtime error: entry point not found.";

            var output = new StringWriter();
            Console.SetOut(output);

            if (!string.IsNullOrWhiteSpace(fakeInput))
                Console.SetIn(new StringReader(fakeInput));

            entry.Invoke(null, null);

            return output.ToString();
        }

        private static string WrapCode(string userCode)
        {
            return $@"using System;

                class Program
                {{
                    static void Main()
                {{
                {userCode}
                }}
                }}";
        }
    }
}