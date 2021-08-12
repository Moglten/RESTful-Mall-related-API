using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    public class SuppliersController : ControllerBase
    {
        private readonly UnitOfWork _unitOfWork = new();
        private readonly IRepository<Supplier> _supplierTb;

        public SuppliersController()
        {
            _supplierTb = _unitOfWork.GetRepositoryInstance<Supplier>();
        }


        // Add Caching Here Due to the operation was executed upon database
        // GET: api/suppliers
        [HttpGet]
        [ResponseCache(Duration = 15)]
        public async Task<ActionResult<IEnumerable<Supplier>>> GetSuppliers(string? sort, string? filter)
        {
            if (sort.Hasvalue() || filter.Hasvalue())
            {
                IQueryable<Supplier> suppliers = _supplierTb.GetAllEntitiesIQueryable();
                var query = new QueryOperations<Supplier>();

                if (filter.Hasvalue())
                {
                    suppliers = query.FiltringTheData(filter, suppliers);
                }
                if (sort.Hasvalue())
                {
                    suppliers = query.SortingTheData(sort, suppliers);
                }
                if (suppliers == null)
                {
                    return BadRequest();
                }
                return await suppliers.ToListAsync();
            }
            return await _supplierTb.GetAllEntitiesIQueryable().ToListAsync();
        }


        //// GET: api/suppliers/paged-supplier
        [HttpGet("paged-supplier")]
        public IActionResult Getsupplier([FromQuery] PagingParamter pagingParameter)
        {

            var supplier = PagedList<Supplier>.ToPagedList(_supplierTb.GetAllEntitiesIQueryable(),
                    pagingParameter.PageNumber,
                    pagingParameter.PageSize);

            var metadata = new
            {
                supplier.TotalCount,
                supplier.PageSize,
                supplier.CurrentPage,
                supplier.TotalPages,
                supplier.HasNext,
                supplier.HasPrevious
            };

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

            return Ok(supplier);
        }


        // GET: api/suppliers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Supplier>> GetSupplier(int id)
        {
            var supplier = _supplierTb.GetEntity(id);

            if (supplier == null)
            {
                return NotFound();
            }

            return supplier;
        }

        // GET: api/suppliers/supplier-details/5
        //Eager loading of supplier related data
        [HttpGet("supplier-details/{id}")]
        public async Task<ActionResult<Supplier>> GetSupplierDetails(int id)
        {
            var supplier = _supplierTb.GetAllEntitiesIQueryable()
                            .Include(supplier => supplier.Products)
                            .Where(supplier => supplier.SupplierID == id)
                            .FirstOrDefault();

            if (supplier == null)
            {
                return NotFound();
            }

            return supplier;
        }

        // PUT: api/suppliers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSupplier(int id, Supplier supplier)
        {
            if (id != supplier.SupplierID)
            {
                return BadRequest();
            }

            _unitOfWork.GetDBInstance().Entry(supplier).State = EntityState.Modified;

            try
            {
                _unitOfWork.SaveChages();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (_supplierTb.GetEntity(id) == null)
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

        // POST: api/suppliers
        [HttpPost]
        public async Task<ActionResult<Supplier>> PostSupplier(Supplier supplier)
        {
            _supplierTb.Add(supplier);
            _unitOfWork.SaveChages();
            return CreatedAtAction("GetSupplier", new { id = supplier.SupplierID }, supplier);
        }

        // DELETE: api/suppliers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSupplier(int id)
        {
            var supplier = _supplierTb.GetEntity(id);
            if (supplier == null)
            {
                return NotFound();
            }

            _supplierTb.Remove(id);
            _unitOfWork.SaveChages();

            return Ok();
        }

        
    }
}
