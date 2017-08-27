using Microsoft.AspNetCore.Mvc;
using SqlServerEFCore.Models;
using System.Diagnostics;
using System.Linq;

namespace SqlServerEFCore.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index([FromServices] TestContext context)
        {
            return View(context.TestData.ToList());
        }
    }
}
