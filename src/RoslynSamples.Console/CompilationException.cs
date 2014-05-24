using System;
using Microsoft.CodeAnalysis.Emit;

namespace RoslynSamples.Console
{
    public class CompilationException : Exception
    {
        public CompilationException(EmitResult compileResult) : base(CreateMessage(compileResult))
        {
        }

        private static string CreateMessage(EmitResult compileResult)
        {
            return "Compile failed" + string.Join(Environment.NewLine, compileResult.Diagnostics);
        }
    }
}