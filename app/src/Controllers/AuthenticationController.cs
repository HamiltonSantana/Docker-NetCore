using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServerSide.Entity;
using ServerSide.Authentication.Interface;
using Microsoft.AspNetCore.Authorization;

namespace ServerSide.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        [AllowAnonymous]
        [HttpPost]
        public IActionResult Auth([FromServices] IAuthenticationService _authService,
            [FromBody] UserEntity usr)
        {
            var tokenResult = _authService.AuthenticationResult(usr);

            if (string.IsNullOrEmpty(tokenResult))
                return Unauthorized();

            return Ok(new {status = 200, token = tokenResult});
        }
        [Authorize]
        [HttpGet]
        public IActionResult CheckAuth()
        {
            return Ok();
        }
    }
}
