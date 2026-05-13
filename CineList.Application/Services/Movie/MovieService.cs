using CineList.Application.Interfaces;
using CineList.Application.Dtos;
using CineList.Domain.Entities;
using CineList.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace CineList.Application.Services
{

    public class MovieService : IMovieService
    {

        private readonly ITmdbService _tmdbService;
        private readonly IUnitOfWork _uow;
        private readonly ILogger<MovieService> _logger;

        public MovieService(ITmdbService tmdbService, IUnitOfWork uow, ILogger<MovieService> logger)
        {

            this._tmdbService = tmdbService;
            this._uow = uow;
            this._logger = logger;
        }

        public async Task<IEnumerable<MovieDto?>> SearchMoviesAsync(string movie)
        {

            var movieExists = await _uow.Movies.GetMovieByTitleAsync(movie);

            if (movieExists.Any())
            {
                return movieExists.Select(m => new MovieDto(
                    TmdbId: m.TmdbId,
                    Title: m.Title,
                    Overview: m.Overview,
                    PosterPath: m.PosterPath,
                    Popularity: m.Popularity
                ));
            }

            var resApi = await _tmdbService.SearchMoviesAsync(movie);

            if (!resApi.Any())
            {
                _logger.LogWarning("Movie {movie} not found ", movie);
                throw new KeyNotFoundException($"Movie {movie} not found ");
            }

            return resApi;

        }

        public async Task<MovieDto?> GetMovieByTmdbIdAsync(int tmdbId)
        {

            var movieExists = await _uow.Movies.GetMovieByTmdbIdAsync(tmdbId);

            if (movieExists != null)
            {
                return new MovieDto(
                   TmdbId: movieExists.TmdbId,
                   Title: movieExists.Title,
                   Overview: movieExists.Overview,
                   PosterPath: movieExists.PosterPath,
                   Popularity: movieExists.Popularity
                );
            }

            var resApi = await _tmdbService.GetMovieByTmdbIdAsync(tmdbId);

            if (resApi == null)
            {
                _logger.LogWarning("Movie {tmdbId} not found", tmdbId);
                throw new KeyNotFoundException($"Movie {tmdbId} not found ");
            }

            var movieModel = new Movie
            {
                Id = Guid.NewGuid(),
                Title = resApi.Title,
                TmdbId = resApi.TmdbId,
                Overview = resApi.Overview,
                PosterPath = resApi.PosterPath,
                Popularity = resApi.Popularity,
                CreatedAt = DateTime.UtcNow
            };

            await _uow.Movies.AddMovieAsync(movieModel);

            return resApi;
        }
    }
}
