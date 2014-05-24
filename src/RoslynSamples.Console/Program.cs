using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoslynSamples.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var code = @"
using System;

public static class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine(""Hello {0}"", args[0]);
    }
}";
            System.Console.WriteLine("Executing code: ");
            System.Console.WriteLine(code);
            System.Console.WriteLine("With argument:  NNUG");
            var codeExecutor = new CodeExecutor();
            codeExecutor.CompileAndExecute(code);
            System.Console.ReadLine();
        }
    }

    public class CodeExecutor
    {
        public void CompileAndExecute(string code)
        {
            throw new NotImplementedException();
        }
    }
}
