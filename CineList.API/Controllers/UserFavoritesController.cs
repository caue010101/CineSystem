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
        public async Task<IActionResult> AddFavoriteMovie(int tmdbId)
        {

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var user = await _userFavoritesService.AddFavoriteAsync(Guid.Parse(userId!), tmdbId);


            return Ok(user);
        }

        [Authorize]
        [HttpGet]

        public async Task<IActionResult> GetFavoriteMovies()
        {

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var user = await _userFavoritesService.GetFavoriteMoviesAsync(Guid.Parse(userId!));

            return Ok(user);

        }

        [Authorize]
        [HttpDelete("{tmdbId}")]

        public async Task<IActionResult> DeleteFavoriteMovie(int tmdbId)
        {

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            await _userFavoritesService.DeleteFavoriteAsync(Guid.Parse(userId!), tmdbId);

            return Ok(new { message = $"Movie {tmdbId} deleted successfuly ", tmdbId });
        }
    }
}
