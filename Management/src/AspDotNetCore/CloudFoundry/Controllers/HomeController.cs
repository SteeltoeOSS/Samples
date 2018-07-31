using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading;
using System;

namespace CloudFoundry.Controllers
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

        public IActionResult Error()
        {
            throw new ArgumentException();
            //return View();
        }
    }
}
