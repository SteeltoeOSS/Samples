using System;
using Microsoft.Extensions.Logging;
using System.Web.Mvc;
using System.Diagnostics;

namespace CloudFoundryWeb.Controllers
{
    public class HomeController : Controller
    {
        private ILogger<HomeController> _logger;

        public HomeController()
        {
            _logger = LoggingConfig.LoggerFactory.CreateLogger<HomeController>();
        }

        // GET: Home
        public ActionResult Index()
        {
            var minlvl = GetMinLogLevel(_logger);
            Console.WriteLine($"Minimum level set on _logger: {minlvl}");
            Debug.WriteLine($"Minimum level set on _logger: {minlvl}");
            _logger.LogTrace("This is a {LogLevel} log", LogLevel.Trace.ToString());
            _logger.LogDebug("This is a {LogLevel} log", LogLevel.Debug.ToString());
            _logger.LogInformation("This is a {LogLevel} log", LogLevel.Information.ToString());
            _logger.LogWarning("This is a {LogLevel} log", LogLevel.Warning.ToString());
            _logger.LogError("This is a {LogLevel} log", LogLevel.Error.ToString());
            _logger.LogCritical("This is a {LogLevel} log", LogLevel.Critical.ToString());
            return View();
        }

        private LogLevel GetMinLogLevel(ILogger logger)
        {
            for (var i = 0; i < 6; i++)
            {
                var level = (LogLevel)Enum.ToObject(typeof(LogLevel), i);
                if (logger.IsEnabled(level))
                {
                    return level;
                }
            }

            return LogLevel.None;
        }
    }
}
