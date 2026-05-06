using CineList.Domain.Interfaces;
using CineList.Domain.Entities;
using Dapper;
using System.Data;

namespace CineList.Infrastructure.Repositories
{

    public class UserRepository : IUserRepository
    {

        private readonly IDbConnection _connection;
        private readonly IUnitOfWork _uow;


        public UserRepository(IDbConnection connection, IUnitOfWork uow)
        {
            this._connection = connection;
            this._uow = uow;
        }

        public async Task<User?> GetUserByIdAsync(Guid id)
        {

            const string sql = @"SELECT
                      id AS Id,
                      name AS Name,
                      email AS Email,
                      password_hash AS PasswordHash,
                      created_at AS CreatedAt
                        FROM users
                          WHERE id = @Id";


            return await _connection.QueryFirstOrDefaultAsync<User>(sql, new { Id = id },
                transaction: _uow.Transaction);

        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {

            const string sql = @"SELECT
                          id AS Id,
                          name AS Name,
                          email AS Email,
                          password_hash AS PasswordHash,
                          created_at AS CreatedAt
                              FROM users
                                WHERE email = @Email;";
            return await _connection.QueryFirstOrDefaultAsync<User>(sql, new { Email = email },
                transaction: _uow.Transaction);

        }

        public async Task AddUserAsync(User user)
        {

            const string sql = @"INSERT INTO users (id, name, email, password_hash, created_at)
                        VALUES(@Id, @Name, @Email, @PasswordHash, @CreatedAt)";

            await _connection.ExecuteAsync(sql, user,
                transaction: _uow.Transaction);
        }

        public async Task UpdateUserAsync(User user)
        {

            const string sql = @"UPDATE users set
                                name = @Name,
                                email = @Email,
                                password_hash = @PasswordHash
                                  WHERE id = @Id";
            await _connection.ExecuteAsync(sql, user,
                transaction: _uow.Transaction);
        }

        public async Task DeleteUserAsync(Guid id)
        {

            const string sql = @"DELETE FROM users WHERE id = @Id";

            await _connection.ExecuteAsync(sql, new { Id = id });
        }
    }
}
