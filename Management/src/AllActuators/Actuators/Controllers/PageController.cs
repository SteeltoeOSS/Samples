using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Steeltoe.Actuators.Models;
using Steeltoe.Actuators.Providers;
using Steeltoe.Actuators.Services;
using Steeltoe.Management.Endpoint.Hypermedia;
using System.Diagnostics;
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
            return View(await logLevelService.GetLogLevelsAndNamespaces());
        }

        public async Task<IActionResult> SetLogLevel()
        {
            return View(await logLevelService.GetLogLevelsAndNamespaces());
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
