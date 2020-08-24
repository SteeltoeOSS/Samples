using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading;
using System;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;

namespace SecureWithUAA.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger _logger;
        private readonly Random _random;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            _random = new Random();
        }

        public IActionResult Index()
        {
            Thread.Sleep(_random.Next(500, 2000));

            _logger.LogCritical("Test Critical message");
            _logger.LogError("Test Error message");
            _logger.LogWarning("Test Warning message");
            _logger.LogInformation("Test Informational message");
            _logger.LogDebug("Test Debug message");
            _logger.LogTrace("Test Trace message");

            return View();
        }
        
        [Authorize(Policy = "fortunes.read")]
        public IActionResult About()
        {
            ViewData["Message"] = "Your About page.";
            return View();
        }

        #region Account-related
        [HttpPost]
        public async Task<IActionResult> LogOff()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        [HttpGet]
        [Authorize]
        public IActionResult Login()
        {
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        public IActionResult Manage()
        {
            ViewData["Message"] = "Manage accounts using UAA or CF command line.";
            return View();
        }

        public IActionResult AccessDenied()
        {
            ViewData["Message"] = "Insufficient permissions.";
            return View();
        }
        #endregion

        public IActionResult Error()
        {
            throw new ArgumentException();
            //return View();
        }
    }
}
