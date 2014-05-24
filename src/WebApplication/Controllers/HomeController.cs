using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RoslynSamples.Console;

namespace WebApplication.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Code = @"
using System;
public static class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine(""Hello World!"");
    }
}";

            return View();
        }

        public ActionResult Compile(string code, string args)
        {
            var arguments = args.Split(new[] {','}).Select(arg => arg.Trim()).ToArray();
            var codeExecutor = new CodeExecutor();
            try
            {
                using (var resultStream = codeExecutor.CompileAndExecute(code, arguments))
                {
                    var reader = new StreamReader(resultStream);
                    ViewBag.Result = reader.ReadToEnd();
                }
            }
            catch (CompilationException ex)
            {
                ViewBag.Result = ex.Message;
            }
            return View("Index");
        }
    }
}