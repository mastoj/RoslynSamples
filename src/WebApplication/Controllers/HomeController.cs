using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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

        public ActionResult Compile()
        {
            throw new NotImplementedException();
        }
    }
}