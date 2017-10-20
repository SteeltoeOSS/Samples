using System.Linq;
using Microsoft.AspNetCore.Mvc;
using MySqlEF6.Models;

namespace MySqlEF6.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult MySqlData([FromServices] TestContext context)
        {
            ViewBag.Database = context.Database.Connection.Database;
            ViewBag.DataSource = context.Database.Connection.DataSource;
            return View(context.TestData.ToList());
        }
    }
}
