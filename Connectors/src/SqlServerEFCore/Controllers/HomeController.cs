using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace SqlServerEFCore.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index([FromServices] TestContext context)
        {
            var connection = context.Database.GetDbConnection();
            Console.WriteLine($"Retrieving data from {connection.DataSource}/{connection.Database}");
            return View(context.TestData.ToList());
        }
    }
}
