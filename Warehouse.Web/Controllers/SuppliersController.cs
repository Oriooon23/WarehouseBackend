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
    public class SuppliersController : ControllerBase
    {
        private readonly ISupplierService _supplierService;

        public SuppliersController(ISupplierService supplierService)
        {
            _supplierService = supplierService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SupplierDTO>>> GetAll()
        {
            var response = await _supplierService.GetAllAsync();
            if (response.Result)
            {
                return Ok(response.Data);
            }
            return StatusCode(response.ErrorCode.GetHttpStatusCode(), response.ErrorMessage);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SupplierDTO>> GetById(int id)
        {
            var response = await _supplierService.GetByIdAsync(id);
            if (response.Result)
            {
                return Ok(response.Data);
            }
            return StatusCode(response.ErrorCode.GetHttpStatusCode(), response.ErrorMessage);
        }

        [HttpPost]
        public async Task<ActionResult<SupplierDTO>> Create([FromBody] SupplierDTO supplierDto)
        {
            if (!User.IsInRole(Policies.Admin))
            {
                return Forbid();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var response = await _supplierService.CreateSupplierAsync(supplierDto);
            if (response.Result)
            {
                return CreatedAtAction(nameof(GetById), new { id = response.Data.Id }, response.Data);
            }
            return StatusCode(response.ErrorCode.GetHttpStatusCode(), response.ErrorMessage);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<SupplierDTO>> Update(int id, [FromBody] SupplierDTO supplierDto)
        {
            if (!User.IsInRole(Policies.Admin) && !User.IsInRole(Policies.Supplier))
            {
                return Forbid();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != supplierDto.Id)
            {
                return BadRequest("L'ID nella richiesta non corrisponde all'ID nel corpo.");
            }
            var response = await _supplierService.UpdateSupplierAsync(id, supplierDto);
            if (response.Result)
            {
                return Ok(response.Data);
            }
            return StatusCode(response.ErrorCode.GetHttpStatusCode(), response.ErrorMessage);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            if (!User.IsInRole(Policies.Admin))
            {
                return Forbid();
            }
            var response = await _supplierService.DeleteSupplierAsync(id);
            if (response.Result)
            {
                return NoContent();
            }
            return StatusCode(response.ErrorCode.GetHttpStatusCode(), response.ErrorMessage);
        }
    }
}