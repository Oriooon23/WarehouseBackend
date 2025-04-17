using Microsoft.AspNetCore.Mvc;
using Warehouse.Common.DTOs;
using Warehouse.Interfaces.ServicesInterfaces;
using Microsoft.AspNetCore.Authorization;
using Warehouse.Common.Security;
using Warehouse.Common.Responses;

namespace Warehouse.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetAll()
        {
            var response = await _productService.GetAllAsync();
            if (response.Result)
            {
                return Ok(response.Data);
            }
            return StatusCode(response.ErrorCode.GetHttpStatusCode(), response.ErrorMessage);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDTO>> GetById(int id)
        {
            var response = await _productService.GetByIdAsync(id);
            if (response.Result)
            {
                return Ok(response.Data);
            }
            return StatusCode(response.ErrorCode.GetHttpStatusCode(), response.ErrorMessage);
        }

        [HttpGet("search")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> Search(string categoryName = null, string productName = null, string supplierName = null)
        {
            var response = await _productService.SearchProductsAsync(categoryName, productName, supplierName);
            if (response.Result)
            {
                return Ok(response.Data);
            }
            return StatusCode(response.ErrorCode.GetHttpStatusCode(), response.ErrorMessage);
        }

        [HttpPost]
        public async Task<ActionResult<ProductDTO>> Create([FromBody] ProductDTO productDto)
        {
            if (!User.IsInRole(Policies.Admin) && !User.IsInRole(Policies.Supplier))
            {
                return Forbid();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var response = await _productService.CreateProductAsync(productDto);
            if (response.Result)
            {
                return CreatedAtAction(nameof(GetById), new { id = response.Data.Id }, response.Data);
            }
            return StatusCode(response.ErrorCode.GetHttpStatusCode(), response.ErrorMessage);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ProductDTO>> Update(int id, [FromBody] ProductDTO productDto)
        {
            if (!User.IsInRole(Policies.Admin) && !User.IsInRole(Policies.Supplier))
            {
                return Forbid();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != productDto.Id)
            {
                return BadRequest("L'ID nella richiesta non corrisponde all'ID nel corpo.");
            }
            var response = await _productService.UpdateProductAsync(id, productDto);
            if (response.Result)
            {
                return Ok(response.Data);
            }
            return StatusCode(response.ErrorCode.GetHttpStatusCode(), response.ErrorMessage);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            if (!User.IsInRole(Policies.Admin) && !User.IsInRole(Policies.Supplier))
            {
                return Forbid();
            }
            var response = await _productService.SoftDeleteProductAsync(id);
            if (response.Result)
            {
                return NoContent();
            }
            return StatusCode(response.ErrorCode.GetHttpStatusCode(), response.ErrorMessage);
        }
    }
}