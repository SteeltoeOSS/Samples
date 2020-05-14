using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace MySqlEFCore.Controllers
{
    public class HomeController : Controller
    {
        private IConfiguration config;

        public HomeController(IConfiguration config)
        {
            this.config = config;
        }

        public IActionResult Index()
        {
            AddMultiDbLink();
            return View();
        }

        public IActionResult MySqlData([FromServices] TestContext context)
        {
            AddMultiDbLink();
            return View(context.TestData.ToList());
        }

        public IActionResult MoreMySqlData([FromServices] SecondTestContext context)
        {
            AddMultiDbLink();
            return View(context.MoreTestData.ToList());
        }

        private void AddMultiDbLink()
        {
            if (config.GetValue<bool>("multipleMySqlDatabases"))
            {
                ViewBag.MultipleDatabases = true;
            }
        }
    }
}