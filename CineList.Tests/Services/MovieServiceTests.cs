using Moq;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using CineList.Application.Interfaces;
using CineList.Domain.Interfaces;
using CineList.Domain.Entities;
using CineList.Application.Services;

namespace CineList.Tests.Services
{

    public class MovieServiceTests
    {

        private readonly Mock<IUnitOfWork> _uowMock;
        private readonly Mock<ILogger<MovieService>> _loggerMock;
        private readonly Mock<IMovieRepository> _movieRepoMock;
        private readonly Mock<ITmdbService> _tmdbServiceMock;
        private readonly MovieService _movieService;

        public MovieServiceTests()
        {
            _uowMock = new Mock<IUnitOfWork>();
            _loggerMock = new Mock<ILogger<MovieService>>();
            _movieRepoMock = new Mock<IMovieRepository>();
            _tmdbServiceMock = new Mock<ITmdbService>();

            _uowMock.Setup(m => m.Movies).Returns(_movieRepoMock.Object);

            _movieService = new MovieService(_tmdbServiceMock.Object, _uowMock.Object, _loggerMock.Object);

        }

        [Fact]

        public async Task SearchMovieByName_ShouldReturnMovieDto_WhenMovieExists()
        {

            var title = "batman";

            _movieRepoMock
              .Setup(m => m.GetMovieByTitleAsync(title))
              .ReturnsAsync(new List<Movie>{
                    new Movie {TmdbId = 49284, Title = title, Overview = "filme legal", PosterPath = "bahbah_ss", Popularity = 39391}
              });

            var result = await _movieService.SearchMoviesAsync(title);

            result.Should().NotBeNull();
            result.First()!.Title.Should().Be(title);

            _tmdbServiceMock.Verify(t => t.SearchMoviesAsync(It.IsAny<string>()), Times.Never());

        }

        [Fact]

        public async Task SearchMovieByName_ShouldThrowKeyNotException_WhenMovieNotExists()
        {

            var title = "batman";

            _movieRepoMock
              .Setup(m => m.GetMovieByTitleAsync(title))
              .ReturnsAsync(new List<Movie> { });

            var act = async () => await _movieService.SearchMoviesAsync(title);

            await act.Should().ThrowAsync<KeyNotFoundException>("Movie not found ");
        }

    }
}
