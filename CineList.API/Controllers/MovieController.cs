using Microsoft.AspNetCore.Mvc;
using CineList.Application.Interfaces;

namespace CineList.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class MovieController : ControllerBase
    {
        private readonly IMovieService _movieService;

        public MovieController(IMovieService movieService)
        {
            this._movieService = movieService;
        }


        [HttpGet("search")]

        public async Task<IActionResult> SearchMovies([FromQuery] string movie)
        {

            var movies = await _movieService.SearchMoviesAsync(movie);

            return Ok(movies);
        }
    }
}
