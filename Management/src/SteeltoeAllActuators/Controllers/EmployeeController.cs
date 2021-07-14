using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using SteeltoeAllActuators.Providers;
using SteeltoeAllActuators.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SteeltoeAllActuators.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly EmployeeData employeeData;

        public EmployeeController(EmployeeData employeeData)
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
