using CineList.Domain.Interfaces;
using System.Data;
using Dapper;
using CineList.Domain.Entities;

namespace CineList.Infrastructure.Repositories
{

    public class UserFavoritesRepository : IUserFavoritesRepository
    {

        private readonly IUnitOfWork _uow;
        private readonly IDbConnection _connection;

        public UserFavoritesRepository(IDbConnection connection, IUnitOfWork uow)
        {

            this._connection = connection;
            this._uow = uow;
        }

        public async Task<Movie?> GetFavoriteMovieAsync(Guid userId, int tmdbId)
        {

            const string sql = @"SELECT
                                m.id AS Id,
                                m.title AS Title,
                                m.tmdb_id AS TmdbId,
                                m.overview AS Overview,
                                m.poster_path AS PosterPath,
                                m.popularity AS Popularity,
                                m.created_at AS CreatedAt
                              FROM user_favorites uf 
                                INNER JOIN movies m ON m.id = uf.movie_id
                                  WHERE uf.user_id = @UserId AND m.tmdb_id = @TmdbId
                                    
                                ";
            return await _connection.QueryFirstOrDefaultAsync<Movie>(sql, new { UserId = userId, TmdbId = tmdbId },
                transaction: _uow.Transaction);
        }

        public async Task<IEnumerable<Movie?>> GetFavoriteMoviesAsync(Guid userId)
        {

            const string sql = @"SELECT
                                m.id AS Id,
                                m.tmdb_id AS TmdbId,
                                m.title AS Title,
                                m.overview AS Overview,
                                m.poster_path AS PosterPath,
                                m.popularity AS Popularity,
                                m.created_at AS CreatedAt
                                  FROM user_favorites uf
                                  INNER JOIN movies m ON m.id = uf.movie_id
                                    WHERE uf.user_id = @UserId";
            return await _connection.QueryAsync<Movie>(sql, new { UserId = userId },
                transaction: _uow.Transaction);
        }

        public async Task AddFavoriteAsync(Guid userId, Guid movieId)
        {

            const string sql = @"INSERT INTO user_favorites (id, user_id, movie_id, created_at)
                                VALUES(@Id, @UserId, @MovieId, @CreatedAt)";

            await _connection.ExecuteAsync(sql,
                new { Id = Guid.NewGuid(), UserId = userId, MovieId = movieId, CreatedAt = DateTime.UtcNow },
                          transaction: _uow.Transaction);
        }

        public async Task<bool> DeleteFavoriteAsync(Guid userId, Guid movieId)
        {

            const string sql = @"DELETE FROM user_favorites
                                 WHERE user_id = @UserId AND movie_id = @MovieId";
            var rowsAffected = await _connection.ExecuteAsync(sql, new { UserId = userId, MovieId = movieId },
                transaction: _uow.Transaction);

            return rowsAffected > 0;
        }
    }
}
