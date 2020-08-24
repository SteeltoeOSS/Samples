using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Placeholder.Models;

namespace Placeholder.Controllers
{
    public class HomeController : Controller
    {
        private IOptionsMonitor<SampleOptions> _opts;

        private SampleOptions Options
        {
            get
            {
                return _opts.CurrentValue;
            }
        }
        public HomeController(IOptionsMonitor<SampleOptions> opts)
        {
            _opts = opts;
        }
        public IActionResult Index()
        {
            ViewData["ResolvedPlaceholderFromEnvVariables"] = Options.ResolvedPlaceholderFromEnvVariables;
            ViewData["ResolvedPlaceholderFromJson"] = Options.ResolvedPlaceholderFromJson;
            ViewData["UnresolvedPlaceholder"] = Options.UnresolvedPlaceholder;
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
