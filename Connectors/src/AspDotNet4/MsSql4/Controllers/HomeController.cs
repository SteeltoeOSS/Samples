using MsSql4.Data;
using System.Linq;
using System.Web.Mvc;

namespace MsSql4.Controllers
{
    public class HomeController : Controller
    {
        private IBloggingContext _context;

        public HomeController(IBloggingContext context)
        {
            _context = context;
        }

        public ActionResult Index()
        {
            var viewModel = new Blog { Name = "Not Initialized" };
            ViewBag.DataSource = _context.GetDatasource();
            ViewBag.Database = _context.GetDatabase();
            viewModel = _context.GetBlogs().First();

            return View(viewModel);
        }
    }
}