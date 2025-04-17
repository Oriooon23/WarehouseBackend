using Microsoft.AspNetCore.Mvc;
using Warehouse.Common.DTOs.UserDTO;
using Warehouse.Common.Responses;
using Warehouse.Services.Services;

namespace Warehouse.Web.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<ResponseBase<AuthResponseDTO>>> Login(UserLoginDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var response = await _authService.LoginAsync(model);
            if (response.Result)
            {
                return Ok(response);
            }
            return Unauthorized(response);
        }

        [HttpPost("register")]
        public async Task<ActionResult<ResponseBase<UserDTO>>> Register(UserRegisterDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var response = await _authService.RegisterAsync(model);
            if (response.Result)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPost("change-password")]
        public async Task<ActionResult<ResponseBase<bool>>> ChangePassword(UserChangePasswordDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var response = await _authService.ChangePasswordAsync(model);
            if (response.Result)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
    }
}