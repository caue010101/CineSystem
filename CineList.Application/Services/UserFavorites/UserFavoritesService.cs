using CineList.Application.Interfaces;
using CineList.Domain.Interfaces;
using CineList.Application.Dtos;
using CineList.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace CineList.Application.Services
{

    public class UserFavoritesService : IUserFavoritesService
    {

        private readonly IUnitOfWork _uow;
        private readonly ITmdbService _tmdbService;
        private readonly ILogger<UserFavoritesService> _logger;

        public UserFavoritesService(IUnitOfWork uow, ITmdbService tmdbService, ILogger<UserFavoritesService> logger)
        {
            this._uow = uow;
            this._tmdbService = tmdbService;
            this._logger = logger;
        }


        public async Task<MovieDto?> AddFavoriteAsync(Guid userId, int tmdbId)
        {
            var alreadyFavorite = await _uow.UserFavorites.GetFavoriteMovieAsync(userId, tmdbId);

            if (alreadyFavorite != null) throw new InvalidOperationException($"Movie is already favorited ");

            var movieEntity = await _uow.Movies.GetMovieByTmdbIdAsync(tmdbId);

            if (movieEntity == null)
            {
                _logger.LogWarning($"Movie not found in database calling api ");
                var api = await _tmdbService.GetMovieByTmdbIdAsync(tmdbId);
                if (api == null)
                    throw new KeyNotFoundException($"Movie {tmdbId} not found ");

                movieEntity = new Movie
                {
                    Id = Guid.NewGuid(),
                    TmdbId = api.TmdbId,
                    Title = api.Title,
                    Overview = api.Overview,
                    PosterPath = api.PosterPath,
                    Popularity = api.Popularity,
                    CreatedAt = DateTime.UtcNow
                };

                _uow.BeginTransaction();
                await _uow.Movies.AddMovieAsync(movieEntity);
                await _uow.UserFavorites.AddFavoriteAsync(userId, movieEntity.Id);
                _uow.Commit();

            }

            else
            {
                await _uow.UserFavorites.AddFavoriteAsync(userId, movieEntity.Id);
            }

            return new MovieDto(
              TmdbId: movieEntity.TmdbId,
              Title: movieEntity.Title,
              Overview: movieEntity.Overview,
              PosterPath: movieEntity.PosterPath,
              Popularity: movieEntity.Popularity
            );

        }
    }
}
