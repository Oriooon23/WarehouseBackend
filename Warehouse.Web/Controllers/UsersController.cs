using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Warehouse.Common.DTOs.UserDTO;
using Warehouse.Common.Responses;
using Warehouse.Interfaces.ServicesInterfaces;
using Warehouse.Common.Security;

namespace Warehouse.Web.Controllers
{
    [ApiController]
    [Route("api/users")]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<ResponseBase<UserDTO>>> Put(int id, [FromBody] UserUpdateDTO model)
        {
            if (model == null)
            {
                return BadRequest(ResponseBase<UserDTO>.Fail("Dati di aggiornamento non validi.", ErrorCode.BadRequest));
            }

            var response = await _userService.UpdateUserProfileAsync(id, model);
            if (response.Result)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = Policies.Admin)]
        public async Task<ActionResult<ResponseBase<bool>>> Delete(int id)
        {
            var response = await _userService.DeleteUserAsync(id);
            if (response.Result)
            {
                return NoContent();
            }
            return NotFound(response);
        }

    }
}