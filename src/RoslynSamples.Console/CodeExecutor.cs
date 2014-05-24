using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace RoslynSamples.Console
{
    public class CodeExecutor
    {
        private static IEnumerable<string> _defaultReferencesNames = new [] { "mscorlib.dll", "System.dll", "System.Core.dll" };
        private static string _assemblyPath = Path.GetDirectoryName(typeof(object).Assembly.Location);
        private static IEnumerable<MetadataFileReference> _defaultReferences =
            _defaultReferencesNames.Select(y => new MetadataFileReference(Path.Combine(_assemblyPath, y)));

        public Stream CompileAndExecute(string code, params string[] args)
        {
            var assembly = CompileCode(code);

            return ExecuteAssembly(assembly, args);
        }

        private Assembly CompileCode(string code)
        {
            // Create the syntax tree
            var syntaxTree = CSharpSyntaxTree.ParseText(code);

            // Compile the syntax tree
            var compilation = CSharpCompilation.Create("Executable", new[] {syntaxTree}, 
                _defaultReferences,
                new CSharpCompilationOptions(OutputKind.ConsoleApplication));

            var assembly = CreateAssembly(compilation);
            return assembly;
        }

        private Assembly CreateAssembly(CSharpCompilation compilation)
        {
            using(var outputStream = new MemoryStream())
            using (var pdbStream = new MemoryStream())
            {
                // Emit assembly to streams
                var result = compilation.Emit(outputStream, pdbStream: pdbStream);
                if (!result.Success)
                {
                    throw new CompilationException(result);
                }

                // Load the compiled assembly;
                var assembly = Assembly.Load(outputStream.ToArray(), pdbStream.ToArray());
                return assembly;
            }
        }

        private Stream ExecuteAssembly(Assembly assembly, string[] args)
        {
            // Set up console out to a memory stream so we can catch the output
            var outStream = new MemoryStream();
            var oldOut = System.Console.Out;
            var writer = new StreamWriter(outStream);
            System.Console.SetOut(writer);

            // Execute the assembly
            assembly.EntryPoint.Invoke(null, new object[] { args });
            writer.Flush();
            
            // Reset the console output
            System.Console.SetOut(oldOut);

            outStream.Position = 0;
            return outStream;
        }
    }
}