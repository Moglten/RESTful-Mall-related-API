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
    public class OrdersController : ControllerBase
    {
        private readonly UnitOfWork _unitOfWork = new();
        private readonly IRepository<Order> _orderTb;

        public OrdersController()
        {
            _orderTb = _unitOfWork.GetRepositoryInstance<Order>();
        }

        // Add Caching Here Due to the operation was executed upon database
        // GET: api/orders
        [HttpGet]
        [ResponseCache(Duration = 15)]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders(string? sort, string? filter)
        {
            if (sort.Hasvalue() || filter.Hasvalue())
            {
                IQueryable<Order> orders = _orderTb.GetAllEntitiesIQueryable();
                var query = new QueryOperations<Order>();

                if (filter.Hasvalue())
                {
                    orders = query.FiltringTheData(filter, orders);
                }
                if (sort.Hasvalue())
                {
                    orders = query.SortingTheData(sort, orders);
                }
                if (orders == null)
                {
                    return BadRequest();
                }

                return await orders.ToListAsync();
            }

            return await _orderTb.GetAllEntitiesIQueryable().ToListAsync();
        }

        // GET: api/orders/paged-orders
        [HttpGet("paged-orders")]
        public IActionResult GetOrders([FromQuery] PagingParamter pagingParameter)
        {

            var ord = PagedList<Order>.ToPagedList(_orderTb.GetAllEntitiesIQueryable(),
                    pagingParameter.PageNumber,
                    pagingParameter.PageSize);

            var metadata = new
            {
                ord.TotalCount,
                ord.PageSize,
                ord.CurrentPage,
                ord.TotalPages,
                ord.HasNext,
                ord.HasPrevious
            };

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

            return Ok(ord);

        }


        // GET: api/orders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            var order = _orderTb.GetEntity(id);

            if (order == null)
            {
                return NotFound();
            }

            return order;
        }

        //Eager loading of order related data
        [HttpGet("order-details-eagerly/{id}")]
        public async Task<ActionResult<Order>> GetShipperDetails(int id)
        {
            var order = _orderTb.GetAllEntitiesIQueryable()
                            .Include(order => order.Order_Details)
                            .Where(order => order.OrderID == id)
                            .FirstOrDefault();

            if (order == null)
            {
                return NotFound();
            }

            return order;
        }

        //Explict loading of order related data
        [HttpGet("order-details-explictly/{id}")]
        public async Task<ActionResult<Order>> EXPGetShipperDetails(int id)
        {
            var order = _orderTb
                        .GetAllEntitiesIQueryable()
                        .Where(order => order.OrderID == id)
                        .SingleOrDefault();

            _unitOfWork.GetDBInstance()
                .Entry(order)
                .Collection(order => order.Order_Details)
                .Load();

            if (order == null)
            {
                return NotFound();
            }

            return order;
        }

        // PUT: api/orders/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder(int id, Order order)
        {
            if (id != order.OrderID)
            {
                return BadRequest();
            }

            _orderTb.Update(order);

            try
            {
                _unitOfWork.SaveChages();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (_orderTb.GetEntity(id)== null)
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

        // POST: api/orders
        [HttpPost]
        public async Task<ActionResult<Order>> PostOrder(Order order)
        {
            _orderTb.Add(order);
            _unitOfWork.SaveChages();

            return CreatedAtAction("GetOrder", new { id = order.OrderID }, order);
        }

        // DELETE: api/orders/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = _orderTb.GetEntity(id);
            if (order == null)
            {
                return NotFound();
            }

            _orderTb.Remove(id);
            _unitOfWork.SaveChages(); 

            return Ok();
        }


    }
}
