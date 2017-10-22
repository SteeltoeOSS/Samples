using FortuneTellerUI4.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace FortuneTellerUI4.Controllers
{
    public class HomeController : Controller
    {
        IFortuneService _fortunes;
        ILogger<HomeController> _logger;

        public HomeController(IFortuneService fortunes,  ILoggerFactory logFactory = null)
        {
            _fortunes = fortunes;
            _logger = logFactory?.CreateLogger<HomeController>();
        }

        public ActionResult Index()
        {
            _logger?.LogInformation("Index");
            return View();
        }

        public async Task<string> Random()
        {
            _logger?.LogInformation("Random");
            var result= await _fortunes.RandomFortuneAsync();
            _logger?.LogInformation("Random returning: " + result);
            return result;
        }
    }
}