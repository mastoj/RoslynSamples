using System;
using System.Collections.Generic;
using System.IO;
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
            try
            {
                using (var resultStream = codeExecutor.CompileAndExecute(code, "NNUG"))
                {
                    System.Console.WriteLine("The application was compiled and executed, press enter to see the result");
                    System.Console.ReadLine();
                    var streamReader = new StreamReader(resultStream);
                    System.Console.WriteLine(streamReader.ReadToEnd());
                }

            }
            catch (CompilationException ex)
            {
                System.Console.WriteLine("Compilation failed: ");
                System.Console.WriteLine(ex.Message);
            }
            System.Console.ReadLine();
        }
    }
}
