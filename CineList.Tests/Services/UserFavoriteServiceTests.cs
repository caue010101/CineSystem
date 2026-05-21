using Moq;
using FluentAssertions;
using CineList.Application.Dtos;
using CineList.Application.Interfaces;
using CineList.Application.Services;
using CineList.Infrastructure.Repositories;
using CineList.Domain.Interfaces;
using CineList.Infrastructure.Service;
using Microsoft.Extensions.Logging;
using CineList.Domain.Entities;

namespace CineList.Tests.Services
{

    public class UserFavoriteServiceTests
    {

        private readonly Mock<ILogger<UserFavoritesService>> _loggerMock;
        private readonly Mock<ITmdbService> _tmdbServiceMock;
        private readonly Mock<IUserFavoritesRepository> _userFavoritesRepoMock;
        private readonly Mock<IUnitOfWork> _uowMock;
        private readonly Mock<IMovieRepository> _movieRepoMock;
        private readonly UserFavoritesService _userFavoritesService;

        public UserFavoriteServiceTests()
        {

            _loggerMock = new Mock<ILogger<UserFavoritesService>>();
            _tmdbServiceMock = new Mock<ITmdbService>();
            _userFavoritesRepoMock = new Mock<IUserFavoritesRepository>();
            _movieRepoMock = new Mock<IMovieRepository>();
            _uowMock = new Mock<IUnitOfWork>();

            _uowMock.Setup(u => u.UserFavorites).Returns(_userFavoritesRepoMock.Object);
            _uowMock.Setup(m => m.Movies).Returns(_movieRepoMock.Object);

            _userFavoritesService =
              new UserFavoritesService(_uowMock.Object, _tmdbServiceMock.Object, _loggerMock.Object);
        }

        [Fact]

        public async Task AddFavoriteMovieAsync_ShoudReturnMovieDto_WhenMovieNotExists()
        {

            var userId = Guid.NewGuid();
            var tmdbId = 556;
            var movieId = Guid.NewGuid();

            _movieRepoMock
              .Setup(m => m.GetMovieByTmdbIdAsync(tmdbId))
              .ReturnsAsync((Movie?)null);

            _userFavoritesRepoMock
              .Setup(m => m.GetFavoriteMovieAsync(userId, tmdbId))
              .ReturnsAsync((Movie?)null);

            _tmdbServiceMock
              .Setup(m => m.GetMovieByTmdbIdAsync(tmdbId))
              .ReturnsAsync(new MovieDto(
                    TmdbId: tmdbId,
                    Title: "batman",
                    Overview: "teste",
                    PosterPath: "/abc.com",
                    Popularity: 100
                ));

            _movieRepoMock
              .Setup(m => m.AddMovieAsync(It.IsAny<Movie>()))
              .Returns(Task.CompletedTask);

            _userFavoritesRepoMock
              .Setup(m => m.AddFavoriteAsync(userId, It.IsAny<Guid>()))
              .Returns(Task.CompletedTask);

            var result = await _userFavoritesService.AddFavoriteAsync(userId, tmdbId);

            result.Should().NotBeNull();
            result.TmdbId.Should().Be(tmdbId);
            result.Title.Should().Be("batman");
        }

        [Fact]

        public async Task AddFavoriteMovieAsync_ShouldThrowInvalidOperationException_WhenMovieExists()
        {

            var userId = Guid.NewGuid();
            var tmdbId = 556;

            _userFavoritesRepoMock
              .Setup(m => m.GetFavoriteMovieAsync(userId, tmdbId))
             .ReturnsAsync(new Movie
             {
                 TmdbId = tmdbId,
                 Title = "batman"
             });

            var act = async () => await _userFavoritesService.AddFavoriteAsync(userId, tmdbId);
            await act.Should().ThrowAsync<InvalidOperationException>()
              .WithMessage("Movie is already favorited ");

        }

        [Fact]

        public async Task GetFavoriteMoviesAsync_ShouldReturnMovieDto_WhenMovieExists()
        {

            var userId = Guid.NewGuid();
            var tmdbId = 678;

            _userFavoritesRepoMock
              .Setup(m => m.GetFavoriteMoviesAsync(userId))
              .ReturnsAsync(new List<Movie>{
                  new Movie {
                    TmdbId = tmdbId,
                    Title = "batman",
                    Overview = "filme ruim",
                    PosterPath = "/test123",
                    Popularity = 134
                  },
              });

            var result = await _userFavoritesService.GetFavoriteMoviesAsync(userId);

            result.Should().NotBeNullOrEmpty();
            result.First().Title.Should().Be("batman");
            result.First().TmdbId.Should().Be(tmdbId);
        }

        [Fact]

        public async Task GetFavoriteMoviesAsync_ShouldReturnEmptyList_WhenMovieNotExists()
        {

            var userId = Guid.NewGuid();

            _userFavoritesRepoMock
              .Setup(m => m.GetFavoriteMoviesAsync(userId))
              .ReturnsAsync(new List<Movie>());

            var result = await _userFavoritesService.GetFavoriteMoviesAsync(userId);

            result.Should().NotBeNull();
        }

        [Fact]

        public async Task DeleteFavoriteMovieAsync_ShouldReturnTrue_WhenMovieExists()
        {

            var userId = Guid.NewGuid();
            var tmdbId = 535;

            _movieRepoMock
              .Setup(m => m.GetMovieByTmdbIdAsync(tmdbId))
              .ReturnsAsync(new Movie { Id = Guid.NewGuid(), TmdbId = tmdbId });

            _userFavoritesRepoMock
              .Setup(m => m.DeleteFavoriteAsync(userId, It.IsAny<Guid>()))
              .ReturnsAsync(true);

            var result = await _userFavoritesService.DeleteFavoriteAsync(userId, tmdbId);

            result.Should().BeTrue();

        }

        [Fact]

        public async Task DeleteFavoriteMovieAsync_ShouldThrowKeyNotInvalidException()
        {
            var userId = Guid.NewGuid();
            var tmdbId = 646;

            _userFavoritesRepoMock
              .Setup(m => m.DeleteFavoriteAsync(userId, It.IsAny<Guid>()))
              .ReturnsAsync(false);

            var result = await _userFavoritesService.DeleteFavoriteAsync(userId, tmdbId);

            result.Should().BeFalse();

        }
    }
}
