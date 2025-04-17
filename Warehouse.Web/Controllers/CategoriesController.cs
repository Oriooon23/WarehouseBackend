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
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetAll()
        {
            var response = await _categoryService.GetAllAsync();
            if (response.Result)
            {
                return Ok(response.Data);
            }
            return StatusCode(response.ErrorCode.GetHttpStatusCode(), response.ErrorMessage);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryDTO>> GetById(int id)
        {
            var response = await _categoryService.GetByIdAsync(id);
            if (response.Result)
            {
                return Ok(response.Data);
            }
            return StatusCode(response.ErrorCode.GetHttpStatusCode(), response.ErrorMessage);
        }

        [HttpPost]
        public async Task<ActionResult<CategoryDTO>> Create([FromBody] CategoryDTO categoryDto)
        {
            if (!User.IsInRole(Policies.Admin))
            {
                return Forbid();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var response = await _categoryService.CreateCategoryAsync(categoryDto);
            if (response.Result)
            {
                return CreatedAtAction(nameof(GetById), new { id = response.Data.Id }, response.Data);
            }
            return StatusCode(response.ErrorCode.GetHttpStatusCode(), response.ErrorMessage);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<CategoryDTO>> Update(int id, [FromBody] CategoryDTO categoryDto)
        {
            if (!User.IsInRole(Policies.Admin))
            {
                return Forbid();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != categoryDto.Id)
            {
                return BadRequest("L'ID nella richiesta non corrisponde all'ID nel corpo.");
            }
            var response = await _categoryService.UpdateCategoryAsync(id, categoryDto);
            if (response.Result)
            {
                return Ok(response.Data);
            }
            return StatusCode(response.ErrorCode.GetHttpStatusCode(), response.ErrorMessage);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id, [FromBody] CategoryDTO categoryDTO)
        {
            if (!User.IsInRole(Policies.Admin))
            {
                return Forbid();
            }
            var response = await _categoryService.DeleteCategoryAsync(id, categoryDTO);
            if (response.Result)
            {
                return NoContent();
            }
            return StatusCode(response.ErrorCode.GetHttpStatusCode(), response.ErrorMessage);
        }
    }
}