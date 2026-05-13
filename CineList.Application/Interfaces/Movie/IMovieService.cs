using CineList.Application.Dtos;

namespace CineList.Application.Interfaces
{

    public interface IMovieService
    {
        Task<IEnumerable<MovieDto?>> SearchMoviesAsync(string movie);
        Task<MovieDto?> GetMovieByTmdbIdAsync(int tmdbId);
    }
}
