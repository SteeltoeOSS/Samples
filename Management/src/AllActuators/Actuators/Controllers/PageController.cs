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

        public async Task<IActionResult> Logging(int pageIndex = 1, string searchFilter = "")
        {
            ViewData["SearchFilter"] = searchFilter;

            var dynamicLogLevels = await logLevelService.GetLogLevelsAndNamespaces();

            var loggingViewModel = new LoggingViewModel(dynamicLogLevels)
            {
                PageIndex = pageIndex,
                SearchKeyword = searchFilter
            };

            return View(loggingViewModel);
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
