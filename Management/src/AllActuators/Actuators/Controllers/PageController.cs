using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Steeltoe.Actuators.Models;
using Steeltoe.Actuators.Providers;
using Steeltoe.Management.Endpoint.Hypermedia;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Steeltoe.Actuators.Controllers
{
    public class PageController : Controller
    {
        private readonly ActuatorEndpoint actuatorEndpoint;
        private readonly EmployeeDataContext employeeDataContext;
        private readonly ILogger<PageController> logger;

        public PageController(
            ActuatorEndpoint actuatorEndpoint,
            EmployeeDataContext employeeDataContext, 
            ILogger<PageController> logger)
        {
            this.actuatorEndpoint = actuatorEndpoint;
            this.employeeDataContext = employeeDataContext;
            this.logger = logger;
        }

        public IActionResult Index()
        {
            logger.LogInformation("Retrieving actuators");

            var actuatorLinks = Enumerable.Empty<HrefProperties>();

            var actuatorEndpoints = actuatorEndpoint.Invoke("/actuator");

            if (actuatorEndpoints is not null)
            {
                logger.LogInformation($"Found {actuatorEndpoints._links.Count} actuators");

                actuatorLinks = actuatorEndpoints._links.Select(link =>
                    new HrefProperties
                    {
                        Display = link.Key != "self" ? link.Key : "all actuators",
                        Address = link.Value.Href
                    });
            }
            else
            {
                logger.LogError("No actuators were found");
            }

            return View(actuatorLinks);
        }

        public IActionResult Migration()
        {
            logger.LogInformation("Retrieving employee data");

            var employees = employeeDataContext?.Employees.ToArray();

            logger.LogInformation($"Found {employees?.Length} employees");

            return View(employees);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
