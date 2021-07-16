using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using Steeltoe.Actuators.Providers;
using Steeltoe.Actuators.Models;

namespace Steeltoe.Actuators.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly EmployeeDataContext employeeData;

        public EmployeeController(EmployeeDataContext employeeData)
        {
            this.employeeData = employeeData;
        }

        [HttpGet]
        public IEnumerable<Employee> Get()
        {
            return employeeData.Employees;
        }

        [HttpGet("{id}")]
        public ActionResult<Employee> Get(string id)
        {
            Employee employee = default;

            if (Guid.TryParse(id, out Guid employeeId))
            {
                employee = employeeData.Employees.Where(employee => employee.Id == employeeId).FirstOrDefault();
            }
            else
            {
                return BadRequest();
            }

            return employee is not null ? Ok(employee) : NotFound();
        }
    }
}
