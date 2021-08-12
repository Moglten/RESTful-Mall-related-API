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
    public class ProductsController : ControllerBase
    {
        private readonly UnitOfWork _unitOfWork = new();
        private readonly IRepository<Product> _productTb;

        public ProductsController()
        {
            _productTb = _unitOfWork.GetRepositoryInstance<Product>();
        }

        // Add Caching Here Due to the operation was executed upon database
        // GET: api/products
        [HttpGet]
        [ResponseCache(Duration = 15)]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts(string? sort, string? filter)
        {
            if (sort.Hasvalue() || filter.Hasvalue())
            {
                IQueryable<Product> products = _productTb.GetAllEntitiesIQueryable();
                var query = new QueryOperations<Product>();

                if (filter.Hasvalue())
                {
                    products = query.FiltringTheData(filter, products);
                }
                if (sort.Hasvalue())
                {
                    products = query.SortingTheData(sort, products);
                }
                if (products == null)
                {
                    return BadRequest();
                }

                return await products.ToListAsync();
            }
            return await _productTb.GetAllEntitiesIQueryable().ToListAsync();
        }


        // GET: api/products/paged-products
        [HttpGet("paged-products")]
        public IActionResult GetProducts([FromQuery] PagingParamter pagingParameter)
        {

            var prod = PagedList<Product>.ToPagedList(_productTb.GetAllEntitiesIQueryable(),
                    pagingParameter.PageNumber,
                    pagingParameter.PageSize);

            var metadata = new
            {
                prod.TotalCount,
                prod.PageSize,
                prod.CurrentPage,
                prod.TotalPages,
                prod.HasNext,
                prod.HasPrevious
            };

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

            return Ok(prod);

        }

        // GET: api/products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = _productTb.GetEntity(id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        // PUT: api/products/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, Product product)
        {
            if (id != product.ProductID)
            {
                return BadRequest();
            }

            _productTb.Update(product);

            try
            {
                _unitOfWork.SaveChages();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (_productTb.GetEntity(id) == null)
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

        // POST: api/products
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            _productTb.Add(product);
            _unitOfWork.SaveChages();

            return CreatedAtAction("GetProduct", new { id = product.ProductID }, product);
        }

        // DELETE: api/products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = _productTb.GetEntity(id);
            if (product == null)
            {
                return NotFound();
            }

            _productTb.Remove(id);
            _unitOfWork.SaveChages();

            return Ok();
        }

    }
}
