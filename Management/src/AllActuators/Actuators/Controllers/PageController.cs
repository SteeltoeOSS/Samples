using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Steeltoe.Actuators.Extensions;
using Steeltoe.Actuators.Models;
using Steeltoe.Actuators.Providers;
using Steeltoe.Actuators.Services;
using System.Diagnostics;
using System.Globalization;
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

        public async Task<IActionResult> Logging(int page = 0, string filter = "")
        {
            ViewData["Filter"] = filter;

            var logLevelsAndNamespaces = await logLevelService.GetLogLevelsAndNamespaces();

            logLevelsAndNamespaces.Filter(filter, page, 10);

            return View(logLevelsAndNamespaces);
        }

        public async Task<IActionResult> SetLogLevel(string logger, string newLevel)
        {
            _ = await logLevelService.SetLogLevels(name: logger, level: newLevel);

            return RedirectToAction("Logging");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
