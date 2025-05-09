using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILogger<EmployeesController> _logger;

        public EmployeesController(
            IEmployeeRepository employeeRepository,
            ILogger<EmployeesController> logger)
        {
            _employeeRepository = employeeRepository;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees()
        {
            _logger.LogInformation("Getting all employees ordered by job title");
            
            var employees = await _employeeRepository.GetAllEmployeesOrderedByJobTitleAsync();
            return Ok(employees);
        }
    }
} 