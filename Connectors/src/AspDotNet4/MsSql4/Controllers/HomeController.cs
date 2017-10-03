using MsSql4.Data;
using System;
using System.Collections.Generic;
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
            var viewModel = new Blog { Name = "Failed to retrieve data...", Posts = new List<Post>() };
            try { 
                ViewBag.DataSource = _context.GetDatasource();
                ViewBag.Database = _context.GetDatabase();
                viewModel = _context.GetBlogs().First();
            }
            catch(Exception e)
            {
                ViewBag.ConnectionString = _context.GetFullConnectionString();
                Console.WriteLine(e.ToString());
            }

            return View(viewModel);
        }
    }
}