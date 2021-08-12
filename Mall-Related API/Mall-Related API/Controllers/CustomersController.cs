using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mall_Related_API.Models;
using Mall_Related_API.Classes;
using Mall_Related_API.Repository;
using NPOI.SS.Formula.Functions;
using Mall_Related_API.QueryOperationClasses;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;

namespace Mall_Related_API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly UnitOfWork _unitOfWork = new();
        private readonly IRepository<Customer> _customerTb;

        public CustomersController()
        {
            _customerTb = _unitOfWork.GetRepositoryInstance<Customer>();
        }

        // Add Caching Here Due to the operation was executed upon database
        // GET: api/customers    
        [HttpGet]
        [ResponseCache(Duration = 15)]
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers(string? sort, string? filter)
        {
            if(sort.Hasvalue()|| filter.Hasvalue())
            {
                IQueryable<Customer> Cutomers = _customerTb.GetAllEntitiesIQueryable();
                var query = new QueryOperations<Customer>();

                if (filter.Hasvalue())
                {
                    Cutomers = query.FiltringTheData(filter, Cutomers);
                }
                if (sort.Hasvalue())
                {
                    Cutomers = query.SortingTheData(sort, Cutomers);
                }
                if (Cutomers == null)
                {
                    return BadRequest();
                }

                return await Cutomers.ToListAsync();
            }

           
            return await _customerTb
                            .GetAllEntitiesIQueryable()
                            .ToListAsync();
        }

        // GET: api/customers/paged-customers  
        [HttpGet("paged-customers")]
        public IActionResult GetCustomers([FromQuery] PagingParamter pagingParameter)
        {

            var cus = PagedList<Customer>.ToPagedList(_customerTb.GetAllEntitiesIQueryable(),
                    pagingParameter.PageNumber,
                    pagingParameter.PageSize);

            var metadata = new
            {
                cus.TotalCount,
                cus.PageSize,
                cus.CurrentPage,
                cus.TotalPages,
                cus.HasNext,
                cus.HasPrevious
            };

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

            return Ok(cus);

        }
    


        // GET: api/customers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> GetCustomer(string id)
        {
            var customer = _customerTb.GetEntity(id);

            if (customer == null)
            { 
                return NotFound();
            }

            return customer;
        }

        // Eager loading related data
        // GET: api/customers/customer-details/5
        [HttpGet("customer-details/{id}")]
        public ActionResult<Customer> GetCustomerDetails(string id)
        {
            var customer = _customerTb
                            .GetAllEntitiesIQueryable()
                            .Include(customer => customer.Orders)
                            .Where(customer => customer.CustomerID == id)
                            .FirstOrDefault();

            if (customer == null)
            {
                return NotFound();
            }

            return customer;
        }


        // PUT: api/customers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomer(string id, Customer customer)
        {
            if (id != customer.CustomerID)
            {
                return BadRequest();
            }

            _customerTb.Update(customer);

            try
            {
                _unitOfWork.SaveChages();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (_customerTb.GetEntity(id) == null)
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

        // POST: api/customers
        [HttpPost]
        public async Task<ActionResult<Customer>> PostCustomer(Customer customer)
        {
            _customerTb.Add(customer);

            try
            {
                _unitOfWork.SaveChages();
            }
            catch (DbUpdateException)
            {
                if (_customerTb.GetEntity(customer.CustomerID) == null)
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetCustomer", new { id = customer.CustomerID }, customer);
        }

        // DELETE: api/customers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(string id)
        {
            var customer =  _customerTb.GetEntity(id);
            if (customer == null)
            {
                return NotFound();
            }

            _customerTb.Remove(id);
            _unitOfWork.SaveChages();

            return Ok();
        }


    }
}
