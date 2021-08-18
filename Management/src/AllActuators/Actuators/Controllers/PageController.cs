using Microsoft.AspNetCore.Mvc;
using Steeltoe.Actuators.Models;
using Steeltoe.Actuators.Providers;
using Steeltoe.Actuators.Services;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Steeltoe.Actuators.Controllers
{
    public class PageController : Controller
    {
        private readonly EmployeeDataContext employeeDataContext;
        private readonly IActuatorLinkService actuatorLinkService;
        private readonly ILogLevelService logLevelService;

        public PageController(
            EmployeeDataContext employeeDataContext,
            IActuatorLinkService actuatorLinkService,
            ILogLevelService logLevelService)
        {
            this.employeeDataContext = employeeDataContext;
            this.actuatorLinkService = actuatorLinkService;
            this.logLevelService = logLevelService;
        }

        public IActionResult Index()
        {
            return View(actuatorLinkService.GetActuatorLinks());
        }

        public IActionResult Migration()
        {
            return View(employeeDataContext?.Employees.ToArray());
        }

        public IActionResult Logging()
        {
            return View();
        }

        public async Task<IActionResult> SetLogLevel(string logger, string newLevel, int pageIndex, string searchFilter = "")
        {
            _ = await logLevelService.SetLogLevels(name: logger, level: newLevel);

            return RedirectToAction("Logging", new { pageIndex, searchFilter });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
