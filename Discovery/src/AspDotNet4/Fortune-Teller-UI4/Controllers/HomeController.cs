using FortuneTellerUI4.Services;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace FortuneTellerUI4.Controllers
{
    public class HomeController : Controller
    {
        IFortuneService _fortunes;
        ILogger<HomeController> _logger;

        public HomeController()
        {
            _fortunes = new FortuneService(DiscoveryConfig.DiscoveryClient, LoggingConfig.LoggerFactory);
            _logger = LoggingConfig.LoggerFactory.CreateLogger<HomeController>();
        }

        public ActionResult Index()
        {
            _logger?.LogInformation("Index");
            return View();
        }

        public async Task<string> Random()
        {
            _logger?.LogInformation("Random");
            return  await _fortunes.RandomFortuneAsync();
        }
    }
}