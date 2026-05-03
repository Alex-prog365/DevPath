using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Collections.Generic;

namespace DevPath.Services
{
    public static class CodeExecutionService
    {
        public static List<string> Execute(string code, string fakeInput)
        {
            var output = new List<string>();

            var wrappedCode = WrapCode(code);

            var syntaxTree = CSharpSyntaxTree.ParseText(wrappedCode);

            var references = AppDomain.CurrentDomain
                .GetAssemblies()
                .Where(a => !a.IsDynamic && !string.IsNullOrEmpty(a.Location))
                .Select(a => MetadataReference.CreateFromFile(a.Location));

            var compilation = CSharpCompilation.Create(
                "UserProgram",
                new[] { syntaxTree },
                references,
                new CSharpCompilationOptions(OutputKind.ConsoleApplication)
            );

            using var ms = new MemoryStream();
            var result = compilation.Emit(ms);

            if (!result.Success)
            {
                output.Add("Execution failed");
                return output;
            }

            ms.Seek(0, SeekOrigin.Begin);
            var assembly = Assembly.Load(ms.ToArray());

            var entry = assembly.EntryPoint;

            var writer = new StringWriter();
            Console.SetOut(writer);

            if (!string.IsNullOrEmpty(fakeInput))
            {
                Console.SetIn(new StringReader(fakeInput));
            }

            if (entry != null)
            {
                entry.Invoke(null, new object[] { new string[] { } });
            }
            else
            {
                output.Add("No entry point found");
            }

            var lines = writer.ToString()
                .Split(Environment.NewLine)
                .Where(l => !string.IsNullOrWhiteSpace(l))
                .ToList();

            return lines;
        }

        private static string WrapCode(string userCode)
        {
            return $@"
            using System;

            class Program

            {{
                static void Main(string[] args)
                {{
                    {userCode}
                }}
            }}";
        }
    }
}