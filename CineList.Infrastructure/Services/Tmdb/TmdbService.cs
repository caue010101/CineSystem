using CineList.Application.Interfaces;
using CineList.Application.Dtos;
using System.Text.Json;
using Microsoft.Extensions.Configuration;

namespace CineList.Infrastructure.Service.Tmdb
{
    public class TmdbService : ITmdbService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public TmdbService(HttpClient httpClient, IConfiguration configuration)
        {
            this._apiKey = configuration["Tmdb:ApiKey"]!;
            this._httpClient = httpClient;
            this._httpClient.BaseAddress = new Uri(configuration["Tmdb:BaseUrl"]!);
        }

        public async Task<IEnumerable<MovieDto?>> SearchMoviesAsync(string movie)
        {

            var response = await _httpClient.GetAsync($"search/movie?api_key={_apiKey}&query={Uri.EscapeDataString(movie)}");

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();


            var result = JsonSerializer.Deserialize<TmdbSearchResponseDto>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return result?.Results.Select(m => new MovieDto(
               TmdbId: m.Id,
               Title: m.Title,
               Overview: m.Overview,
               PosterPath: m.PosterPath,
               Popularity: m.Popularity
            )) ?? Enumerable.Empty<MovieDto?>();
        }

        public async Task<MovieDto?> GetMovieByTmdbIdAsync(int tmdbId)
        {

            var response = await _httpClient.GetAsync($"movie/{tmdbId}?api_key={_apiKey}");

            if (!response.IsSuccessStatusCode) return null;

            var content = await response.Content.ReadAsStringAsync();

            var tmdb = JsonSerializer.Deserialize<TmdbResponseDto>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (tmdb == null) return null;

            return new MovieDto(
                TmdbId: tmdb.Id,
                Title: tmdb.Title,
                Overview: tmdb.Overview,
                PosterPath: tmdb.PosterPath,
                Popularity: tmdb.Popularity
            );
        }
    }
}
