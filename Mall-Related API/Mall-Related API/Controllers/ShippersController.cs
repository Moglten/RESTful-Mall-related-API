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
using NPOI.SS.Formula.Functions;
using Mall_Related_API.QueryOperationClasses;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;

namespace Mall_Related_API.Controllers
{

    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ShippersController : ControllerBase
    {
        private readonly UnitOfWork _unitOfWork = new();
        private readonly IRepository<Shipper> _shipperTb;

        public ShippersController()
        {
            _shipperTb = _unitOfWork.GetRepositoryInstance<Shipper>();
        }

        // Add Caching Here Due to the operation was executed upon database
        // GET: api/shippers
        [HttpGet]
        [ResponseCache(Duration = 15)]
        public async Task<ActionResult<IEnumerable<Shipper>>> GetShippers(string? sort, string? filter)
        {
            if (sort.Hasvalue() || filter.Hasvalue())
            {
                IQueryable<Shipper> shippers = _shipperTb.GetAllEntitiesIQueryable();
                var query = new QueryOperations<Shipper>();

                if (filter.Hasvalue())
                {
                    shippers = query.FiltringTheData(filter, shippers);
                }
                if (sort.Hasvalue())
                {
                    shippers = query.SortingTheData(sort, shippers);
                }
                if (shippers == null)
                {
                    return BadRequest();
                }
                return await shippers.ToListAsync();
            }

            return await _shipperTb.GetAllEntitiesIQueryable().ToListAsync();
        }

        //GET: api/shippers/paged-shipper
        [HttpGet("paged-shipper")]
        public IActionResult Getshipper([FromQuery] PagingParamter pagingParameter)
        {

            var shiper = PagedList<Shipper>.ToPagedList(_shipperTb.GetAllEntitiesIQueryable(),
                    pagingParameter.PageNumber,
                    pagingParameter.PageSize);

            var metadata = new
            {
                shiper.TotalCount,
                shiper.PageSize,
                shiper.CurrentPage,
                shiper.TotalPages,
                shiper.HasNext,
                shiper.HasPrevious
            };

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

            return Ok(shiper);
        }

        // GET:
        // api/shippers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Shipper>> GetShipper(int id)
        {
            var shipper = _shipperTb.GetEntity(id);

            if (shipper == null)
            {
                return NotFound();
            }

            return shipper;
        }

        //Eager loading of shipper related data
        [HttpGet("shipper-details-eagerly/{id}")]
        public async Task<ActionResult<Shipper>> EAGGetShipperDetails(int id)
        {
            var shipper = _shipperTb.GetEntity(id);

            if (shipper == null)
            {
                return NotFound();
            }

            return shipper;
        }

        //Explict loading of shipper related data
        [HttpGet("shipper-details-explictly/{id}")]
        public async Task<ActionResult<Shipper>> eXPGetShipperDetails(int id)
        {
            var shipper = _shipperTb.GetEntity(id);

            _unitOfWork.GetDBInstance().Entry(shipper);
                                   

            if (shipper == null)
            {
                return NotFound();
            }

            return shipper;
        }



        // PUT: api/shippers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutShipper(int id, Shipper shipper)
        {
            if (id != shipper.ShipperID)
            {
                return BadRequest();
            }

            _unitOfWork.GetDBInstance().Entry(shipper).State = EntityState.Modified;

            try
            {
                _unitOfWork.SaveChages();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (_shipperTb.GetEntity(id) == null)
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

        // POST: api/shippers
        [HttpPost]
        public async Task<ActionResult<Shipper>> PostShipper(Shipper shipper)
        {
            _shipperTb.Add(shipper);
            _unitOfWork.SaveChages();

            return CreatedAtAction("GetShipper", new { id = shipper.ShipperID }, shipper);
        }

        // DELETE: api/shippers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteShipper(int id)
        {
            var shipper = _shipperTb.GetEntity(id);
            if (shipper == null)
            {
                return NotFound();
            }

            _shipperTb.Remove(id);
            _unitOfWork.SaveChages();

            return Ok();
        }

    }
}
