using Microsoft.AspNetCore.Mvc;
using CineList.Application.Interfaces;
using CineList.Application.Dtos;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace CineList.API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]

    public class UserController : ControllerBase
    {

        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            this._userService = userService;
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> GetUser()
        {

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var user = await _userService.GetUserByIdAsync(Guid.Parse(userId!));

            return Ok(user);
        }

        [HttpPost]

        public async Task<IActionResult> AddUser([FromBody] CreateUserDto dto)
        {

            var userAdd = await _userService.AddUserAsync(dto);

            return StatusCode(201, userAdd);
        }

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> UpdateUserAsync([FromBody] UpdateUserDto dto)
        {

            var userClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var user = await _userService.UpdateUserAsync(Guid.Parse(userClaim!), dto);

            return Ok(user);
        }

        [Authorize]
        [HttpDelete]

        public async Task<IActionResult> DeleteUser()
        {

            var userClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var user = await _userService.DeleteUserAsync(Guid.Parse(userClaim!));

            return NoContent();
        }

    }

}
