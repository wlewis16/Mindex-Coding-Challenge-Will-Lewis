using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using challenge.Models;
using challenge.Services;

namespace challenge.Controllers
{
    [Route("api/compensation")]
    public class CompensationController : Controller
    {
        private readonly ILogger _logger;
        private readonly ICompensationService _compensationService;
        private readonly IEmployeeService _employeeService;

        public CompensationController(ILogger<CompensationController> logger, ICompensationService compensationService, IEmployeeService employeeService)
        {
            _logger = logger;
            _compensationService = compensationService;
            _employeeService = employeeService;
        }

        [HttpPost("{id}")]
        public IActionResult CreateCompensation(String id, [FromBody] Compensation compensation)
        {
            if (String.IsNullOrEmpty(id) || compensation == null)
                return NotFound();

            Employee employee = _employeeService.GetById(id);

            if (employee == null)
                return NotFound();

            compensation.employee = employee;

            _logger.LogDebug($"Received compensation create request for '{compensation.employee.FirstName} {compensation.employee.LastName}'");

            _compensationService.Create(compensation);

            return CreatedAtRoute("getCompensationById", new { id = compensation.employee.EmployeeId }, compensation);
        }

        [HttpGet("{id}", Name = "getCompensationById")]
        public IActionResult GetCompensationById(String id)
        {
            _logger.LogDebug($"Received compensation get request for '{id}'");

            var compensation = _compensationService.GetById(id);

            if (compensation == null)
                return NotFound();

            return Ok(compensation);
        }
    }
}
