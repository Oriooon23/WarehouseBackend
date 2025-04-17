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
    public class CitiesController : ControllerBase
    {
        private readonly ICityService _cityService;

        public CitiesController(ICityService cityService)
        {
            _cityService = cityService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CityDTO>>> GetAll()
        {
            var response = await _cityService.GetAllAsync();
            if (response.Result)
            {
                return Ok(response.Data);
            }
            return StatusCode(response.ErrorCode.GetHttpStatusCode(), response.ErrorMessage);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CityDTO>> GetById(int id)
        {
            var response = await _cityService.GetByIdAsync(id);
            if (response.Result)
            {
                return Ok(response.Data);
            }
            return StatusCode(response.ErrorCode.GetHttpStatusCode(), response.ErrorMessage);
        }

        [HttpPost]
        public async Task<ActionResult<CityDTO>> Create([FromBody] CityDTO cityDto)
        {
            if (!User.IsInRole(Policies.Admin))
            {
                return Forbid();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var response = await _cityService.CreateCityAsync(cityDto);
            if (response.Result)
            {
                return CreatedAtAction(nameof(GetById), new { id = response.Data.Id }, response.Data);
            }
            return StatusCode(response.ErrorCode.GetHttpStatusCode(), response.ErrorMessage);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<CityDTO>> Update(int id, [FromBody] CityDTO cityDto)
        {
            if (!User.IsInRole(Policies.Admin))
            {
                return Forbid();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != cityDto.Id)
            {
                return BadRequest("L'ID nella richiesta non corrisponde all'ID nel corpo.");
            }
            var response = await _cityService.UpdateCityAsync(id, cityDto);
            if (response.Result)
            {
                return Ok(response.Data);
            }
            return StatusCode(response.ErrorCode.GetHttpStatusCode(), response.ErrorMessage);
        }
    }
}