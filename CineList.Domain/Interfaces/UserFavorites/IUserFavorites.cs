using CineList.Domain.Entities;

namespace CineList.Domain.Interfaces
{

    public interface IUserFavoritesRepository
    {
        Task<Movie?> GetFavoriteMovieAsync(Guid userId, int tmdbId);
        Task AddFavoriteAsync(Guid userId, Guid movieId);
    }
}
