using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace MySqlEFCore.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult MySqlData([FromServices] TestContext context)
        {
            return View(context.TestData.ToList());
        }
    }
}