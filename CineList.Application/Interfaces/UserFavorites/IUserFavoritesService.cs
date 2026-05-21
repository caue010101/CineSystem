using CineList.Application.Dtos;

namespace CineList.Application.Interfaces
{

    public interface IUserFavoritesService
    {

        Task<MovieDto?> AddFavoriteAsync(Guid userId, int tmdbId);
        Task<IEnumerable<MovieDto?>> GetFavoriteMoviesAsync(Guid userId);
        Task<bool> DeleteFavoriteAsync(Guid userId, int tmdbId);


    }
}
