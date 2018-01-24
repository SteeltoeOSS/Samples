using System.Linq;
using Microsoft.AspNetCore.Mvc;

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
