using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace SqlServerEFCore.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult SqlServerData([FromServices] TestContext context)
        {
            return View(context.TestData.ToList());
        }
    }
}
