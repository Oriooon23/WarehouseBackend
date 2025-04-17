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
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDTO>>> GetAll()
        {
            if (!User.IsInRole(Policies.Admin))
            {
                return Forbid();
            }
            var response = await _orderService.GetAllAsync();
            if (response.Result)
            {
                return Ok(response.Data);
            }
            return StatusCode(response.ErrorCode.GetHttpStatusCode(), response.ErrorMessage);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<OrderDTO>> GetById(int id)
        {
            var response = await _orderService.GetByIdAsync(id);
            if (response.Result)
            {
                return Ok(response.Data);
            }
            return StatusCode(response.ErrorCode.GetHttpStatusCode(), response.ErrorMessage);
        }

        [HttpPost]
        public async Task<ActionResult<OrderDTO>> Create([FromBody] OrderDTO orderDto)
        {
            if (!User.IsInRole(Policies.User) && !User.IsInRole(Policies.Supplier))
            {
                return Forbid();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var response = await _orderService.CreateOrderAsync(orderDto);
            if (response.Result)
            {
                return CreatedAtAction(nameof(GetById), new { id = response.Data.Id }, response.Data);
            }
            return StatusCode(response.ErrorCode.GetHttpStatusCode(), response.ErrorMessage);
        }

        [HttpPut("{id}/cancel")]
        public async Task<ActionResult<OrderDTO>> Cancel(int id)
        {
            if (!User.IsInRole(Policies.User) && !User.IsInRole(Policies.Supplier))
            {
                return Forbid();
            }
            var response = await _orderService.UpdateOrderStatusAsync(id);
            if (response.Result)
            {
                return Ok(response.Data);
            }
            return StatusCode(response.ErrorCode.GetHttpStatusCode(), response.ErrorMessage);
        }
    }
}