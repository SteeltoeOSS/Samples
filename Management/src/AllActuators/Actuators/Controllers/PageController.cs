using Microsoft.AspNetCore.Mvc;
using Steeltoe.Actuators.Models;
using Steeltoe.Actuators.Providers;
using Steeltoe.Actuators.Services;
using System.Diagnostics;
using System.Linq;

namespace Steeltoe.Actuators.Controllers
{
    public class PageController : Controller
    {
        private readonly EmployeeDataContext employeeDataContext;
        private readonly IActuatorLinkService actuatorLinkService;

        public PageController(
            EmployeeDataContext employeeDataContext,
            IActuatorLinkService actuatorLinkService)
        {
            this.employeeDataContext = employeeDataContext;
            this.actuatorLinkService = actuatorLinkService;
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
