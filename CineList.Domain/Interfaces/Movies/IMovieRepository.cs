using CineList.Domain.Entities;

namespace CineList.Domain.Interfaces
{

    public interface IMovieRepository
    {

        Task<Movie?> GetMovieByIdAsync(Guid id);
        Task<IEnumerable<Movie>> GetMovieByTitleAsync(string movie);
        Task<Movie?> GetMovieByTmdbIdAsync(int tmdbId);
        Task AddMovieAsync(Movie movie);
    }
}
