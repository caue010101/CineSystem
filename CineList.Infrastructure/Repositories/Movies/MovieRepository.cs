using CineList.Domain.Interfaces;
using Dapper;
using System.Data;
using CineList.Domain.Entities;

namespace CineList.Infrastructure.Repositories
{

    public class MovieRepository : IMovieRepository
    {

        private readonly IUnitOfWork _uow;
        private readonly IDbConnection _connection;


        public MovieRepository(IDbConnection connection, IUnitOfWork uow)
        {
            this._connection = connection;
            this._uow = uow;
        }

        public async Task<Movie?> GetMovieByIdAsync(Guid id)
        {



            const string sql = @"SELECT
                                id AS Id,
                                title AS Title,
                                tmdb_id AS TmdbId,
                                overview AS Overview,
                                poster_path AS PosterPath,
                                popularity AS Popularity,
                                created_at AS CreatedAt
                                  FROM movies
                                    WHERE id = @Id";
            return await _connection.QueryFirstOrDefaultAsync<Movie>(sql, new { Id = id },
                transaction: _uow.Transaction);
        }

        public async Task<Movie?> GetMovieByTmdbIdAsync(int tmdbId)
        {

            const string sql = @"SELECT
                                id AS Id,
                                title AS Title,
                                tmdb_id AS TmdbId,
                                overview AS Overview,
                                poster_path AS PosterPath,
                                popularity AS Popularity,
                                created_at AS CreatedAt
                                  FROM movies
                                    WHERE tmdb_id = @TmdbId";
            return await _connection.QueryFirstOrDefaultAsync<Movie>(sql, new { TmdbId = tmdbId },
                transaction: _uow.Transaction);
        }

        public async Task<IEnumerable<Movie>> GetMovieByTitleAsync(string title)
        {

            const string sql = @"SELECT
                                  id AS Id,
                                  title AS Title,
                                  tmdb_id AS TmdbId,
                                  overview AS Overview,
                                  poster_path AS PosterPath,
                                  popularity AS Popularity,
                                  created_at AS CreatedAt
                                      FROM movies
                                        WHERE title ILIKE @Title";

            return await _connection.QueryAsync<Movie>(sql, new { Title = title },
                transaction: _uow.Transaction);
        }

        public async Task AddMovieAsync(Movie movie)
        {

            const string sql = @"INSERT INTO movies (id, title, tmdb_id, overview, poster_path, popularity, created_at)
                                VALUES(@Id, @Title, @TmdbId, @Overview, @PosterPath, @Popularity, @CreatedAt)";

            await _connection.ExecuteAsync(sql, movie,
                transaction: _uow.Transaction);
        }
    }
}
