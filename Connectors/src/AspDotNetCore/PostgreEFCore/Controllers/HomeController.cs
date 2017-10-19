using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace PostgreEFCore.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult PostgresData([FromServices] TestContext context)
        {
            return View(context.TestData.ToList());
        }
    }
}
