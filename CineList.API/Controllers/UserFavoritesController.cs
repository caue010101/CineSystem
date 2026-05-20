using Microsoft.AspNetCore.Mvc;
using CineList.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace CineList.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class UserFavoritesController : ControllerBase
    {
        private readonly IUserFavoritesService _userFavoritesService;

        public UserFavoritesController(IUserFavoritesService userFavoritesService)
        {
            this._userFavoritesService = userFavoritesService;
        }

        [Authorize]
        [HttpPost("{tmdbId}")]
        public async Task<IActionResult> AddFavoriteAsync(int tmdbId)
        {

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var user = await _userFavoritesService.AddFavoriteAsync(Guid.Parse(userId!), tmdbId);


            return Ok(user);
        }

        [Authorize]
        [HttpGet("claims")]

        public async Task<IActionResult> GetClaimAsync()
        {

            var claims = User.Claims.Select(c => new { c.Value, c.Type });

            return Ok(claims);
        }
    }
}
