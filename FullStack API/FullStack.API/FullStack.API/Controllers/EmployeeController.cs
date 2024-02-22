using FullStack.API.Data;
using FullStack.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FullStack.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : Controller
    {

        private readonly FullStackDbContext _db_context;

        public EmployeeController(FullStackDbContext fullStackDbContext)
        {
            this._db_context = fullStackDbContext;
        }
        [HttpGet]
        [Route("GetAllEmployee")]
        public async Task<IActionResult> GetAllEmployee()
        {
            var employee=await _db_context.Employees.ToListAsync();
            return Ok(employee);
        }

        [HttpPost]
        [Route("AddEmployee")]
        public async Task<IActionResult> AddEmployee([FromBody]Employee employeeRequest)
        {
            employeeRequest.Id= Guid.NewGuid();
            await _db_context.Employees.AddAsync(employeeRequest);    
            await _db_context.SaveChangesAsync();   
            return Ok(employeeRequest);
        }

        [HttpGet]
        //[Route("{id:Guid}")]
        [Route("GetEmployee/{id:guid}")]
        public async Task<IActionResult> GetEmployee([FromRoute] Guid id)
        {
            var employee = await _db_context.Employees.FirstOrDefaultAsync(x => x.Id==id);
            if (employee==null)
            {
                return NotFound();
            }
            return Ok(employee);
        }

        [HttpPut]
        //[Route("{id:Guid}")]
        [Route("UpdateEmployee/{id:guid}")]
        public async Task<IActionResult> UpdateEmployee([FromRoute] Guid id,Employee updateEmployeeRequest)
        {
            var employee = await _db_context.Employees.FindAsync(id);
            if (employee==null)
            {
                return NotFound();
            }
            employee.Name = updateEmployeeRequest.Name;
            employee.Email = updateEmployeeRequest.Email;
            employee.Salary = updateEmployeeRequest.Salary;
            employee.Phone = updateEmployeeRequest.Phone;
            employee.Department = updateEmployeeRequest.Department;

            await _db_context.SaveChangesAsync();
            return Ok(employee);
        }

        [HttpDelete]
        //[Route("{id:Guid}")]
        [Route("DeleteEmployee/{id:guid}")]
        public async Task<IActionResult> DeleteEmployee([FromRoute] Guid id)
        {
            var employee = await _db_context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            _db_context.Employees.Remove(employee);
            await _db_context.SaveChangesAsync();
            return Ok(employee); 
        }
    } 
}
