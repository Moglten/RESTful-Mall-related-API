using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mall_Related_API.Models;
using Mall_Related_API.Repository;
using Mall_Related_API.Classes;
using Mall_Related_API.QueryOperationClasses;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;

namespace Mall_Related_API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly UnitOfWork _unitOfWork = new();
        private readonly IRepository<Employee> _EmpTb;

        public EmployeesController()
        {
            _EmpTb = _unitOfWork.GetRepositoryInstance<Employee>();
        }

        // Add Caching Here Due to the operation was executed upon database
        // GET: api/employees
        [HttpGet]
        [ResponseCache(Duration = 15)]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees(string? sort, string? filter)
        {
            if (sort.Hasvalue() || filter.Hasvalue())
            {
                IQueryable<Employee> employees = _EmpTb.GetAllEntitiesIQueryable();
                var query = new QueryOperations<Employee>();

                if (filter.Hasvalue())
                {
                    employees = query.FiltringTheData(filter, employees);
                }
                if (sort.Hasvalue())
                {
                    employees = query.SortingTheData(sort, employees);
                }
                if (employees == null)
                {
                    return BadRequest();
                }

                return await employees.ToListAsync();
            }

            return await _EmpTb.GetAllEntitiesIQueryable().ToListAsync();
        }


        //GET: api/employees/paged-employees
        [HttpGet("paged-employees")]
        public IActionResult GetEmployees([FromQuery] PagingParamter pagingParameter)
        {

            var emp = PagedList<Employee>.ToPagedList(_EmpTb.GetAllEntitiesIQueryable(),
                    pagingParameter.PageNumber,
                    pagingParameter.PageSize);

            var metadata = new
            {
                emp.TotalCount,
                emp.PageSize,
                emp.CurrentPage,
                emp.TotalPages,
                emp.HasNext,
                emp.HasPrevious
            };

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

            return Ok(emp);

        }


        // GET: api/employees/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetEmployee(int id)
        {
            var employee = _EmpTb.GetEntity(id);

            if (employee == null)
            {
                return NotFound();
            }

            return employee;
        }

        // PUT: api/employees/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployee(int id, Employee employee)
        {
            if (id != employee.EmployeeID)
            {
                return BadRequest();
            }

            _EmpTb.Update(employee);

            try
            {
                 _unitOfWork.SaveChages();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (_EmpTb.GetEntity(id) == null)
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok();
        }

        // POST: api/employees
        [HttpPost]
        public async Task<ActionResult<Employee>> PostEmployee(Employee employee)
        {
            _EmpTb.Add(employee);
            _unitOfWork.SaveChages();
            return CreatedAtAction("GetEmployee", new { id = employee.EmployeeID }, employee);
        }

        // DELETE: api/employees/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var employee = _EmpTb.GetEntity(id);
            if (employee == null)
            {
                return NotFound();
            }

            _EmpTb.Remove(id);
            _unitOfWork.SaveChages();

            return Ok();
        }


    }
}
