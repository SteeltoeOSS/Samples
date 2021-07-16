using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using Steeltoe.Actuators.Providers;
using Steeltoe.Actuators.Models;
using Microsoft.Extensions.Logging;

namespace Steeltoe.Actuators.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly EmployeeDataContext employeeData;
        private readonly ILogger<EmployeeController> logger;

        public EmployeeController(EmployeeDataContext employeeData, ILogger<EmployeeController> logger)
        {
            this.employeeData = employeeData;
            this.logger = logger;
        }

        [HttpGet]
        public IEnumerable<Employee> Get()
        {
            logger.LogInformation("Retrieving all employees listed");

            return employeeData.Employees;
        }

        [HttpGet("{id}")]
        public ActionResult<Employee> Get(string id)
        {
            Employee employee = default;

            if (Guid.TryParse(id, out Guid employeeId))
            {
                logger.LogInformation($"Retrieving employee with id {employeeId}");

                employee = employeeData.Employees.Where(employee => employee.Id == employeeId).FirstOrDefault();

                if (employee is null)
                {
                    logger.LogError($"Employee with id {employeeId} was not found");

                    return NotFound();
                }
                else
                {
                    logger.LogInformation($"Found employee with id {employeeId}");

                    return Ok(employee);
                }
            }
            else
            {
                logger.LogError($"Employee ID {id} was not in proper format");

                return BadRequest();
            }
        }
    }
}
