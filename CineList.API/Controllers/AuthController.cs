using Microsoft.AspNetCore.Mvc;
using CineList.Application.Dtos;
using CineList.Application.Interfaces.Auth;

namespace CineList.API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]

    public class AuthController : ControllerBase
    {
        private readonly IAuthService _auth;

        public AuthController(IAuthService auth)
        {
            this._auth = auth;
        }

        [HttpPost("login")]
        public async Task<IActionResult> AuthUser(LoginUserDto dto)
        {

            var token = await _auth.AuthUserAsync(dto);

            return Ok(new { token });
        }
    }
}
