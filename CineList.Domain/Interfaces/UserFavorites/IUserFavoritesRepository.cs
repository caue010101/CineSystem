using CineList.Domain.Entities;

namespace CineList.Domain.Interfaces
{

    public interface IUserFavoritesRepository
    {
        Task<Movie?> GetFavoriteMovieAsync(Guid userId, int tmdbId);
        Task<IEnumerable<Movie?>> GetFavoriteMoviesAsync(Guid userId);
        Task AddFavoriteAsync(Guid userId, Guid movieId);
        Task<bool> DeleteFavoriteAsync(Guid userId, Guid movieId);
    }
}
