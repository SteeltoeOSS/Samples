using MsSql4.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace MsSql4.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var viewModel = new Blog { Name = "Not Initialized" };
            using (var db = new BloggingContext())
            {
                ViewBag.DataSource = db.Database.Connection.DataSource;
                ViewBag.Database = db.Database.Connection.Database;
                viewModel = db.Blogs.Include(p => p.Posts).First();
            }

            return View(viewModel);
        }
    }
}