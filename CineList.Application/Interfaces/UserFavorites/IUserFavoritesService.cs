using CineList.Application.Dtos;

namespace CineList.Application.Interfaces
{

    public interface IUserFavoritesService
    {

        Task<MovieDto?> AddFavoriteAsync(Guid userId, int tmdbId);

    }
}
