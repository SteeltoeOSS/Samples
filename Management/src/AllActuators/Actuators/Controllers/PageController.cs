using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Steeltoe.Actuators.Models;
using Steeltoe.Actuators.Providers;
using Steeltoe.Actuators.Services;
using Steeltoe.Management.Endpoint.Hypermedia;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net.Http;
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

        public async Task<IActionResult> Logging()
        {
            var logLevelsAndNamespaces = await logLevelService.GetLogLevelsAndNamespaces();

            return View(GetLoggingViewModel(logLevelsAndNamespaces));
        }

        public async Task<IActionResult> SetLogLevel(LoggingViewModel loggingViewModel)
        {
            var logNamespace = await logLevelService.SetLogLevels(
                name: loggingViewModel.SelectedNamespace, 
                level: loggingViewModel.SelectedLevel);

            return View(new LoggingViewModel { 
                SelectedNamespace = loggingViewModel.SelectedNamespace,
                SelectedLevel = logNamespace.ConfiguredLevel 
            });
        }

        private static LoggingViewModel GetLoggingViewModel(LogLevelsAndNamespaces logLevelsAndNamespaces)
        {
            var levels = logLevelsAndNamespaces.Levels.Select(level =>
                new SelectListItem
                {
                    Text = new CultureInfo("en-US", false).TextInfo.ToTitleCase(level),
                    Value = level
                }).ToList();

            var namespaces = logLevelsAndNamespaces.Loggers.Select(kvp =>
                new SelectListItem
                {
                    Text = $"{kvp.Key} (Current: {kvp.Value.EffectiveLevel})",
                    Value = kvp.Key
                }).ToList();

            return new LoggingViewModel
            {
                Levels = levels,
                Namespaces = namespaces
            };
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
