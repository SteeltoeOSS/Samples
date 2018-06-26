using Management.App_Start;
using Microsoft.Extensions.Logging;
using System.Web.Mvc;

namespace Management.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            var controllerLogger = ApplicationConfig.LoggerFactory.CreateLogger<HomeController>();
            controllerLogger.LogTrace("This is a {LogLevel} log", LogLevel.Trace.ToString());
            controllerLogger.LogDebug("This is a {LogLevel} log", LogLevel.Debug.ToString());
            controllerLogger.LogInformation("This is a {LogLevel} log", LogLevel.Information.ToString());
            controllerLogger.LogWarning("This is a {LogLevel} log", LogLevel.Warning.ToString());
            controllerLogger.LogError("This is a {LogLevel} log", LogLevel.Error.ToString());
            controllerLogger.LogCritical("This is a {LogLevel} log", LogLevel.Critical.ToString());
            return View();
        }
    }
}