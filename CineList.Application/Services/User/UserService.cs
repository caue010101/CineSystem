using CineList.Application.Interfaces;
using CineList.Application.Dtos;
using CineList.Domain.Interfaces;
using CineList.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace CineList.Application.Services
{

    public class UserService : IUserService
    {

        private readonly IUnitOfWork _uow;
        private readonly ILogger<UserService> _logger;

        public UserService(IUnitOfWork uow, ILogger<UserService> logger)
        {
            this._uow = uow;
            this._logger = logger;
        }

        public async Task<ReadUserDto?> GetUserByIdAsync(Guid id)
        {

            var user = await _uow.Users.GetUserByIdAsync(id);

            if (user == null)


            {
                _logger.LogWarning("User {id} not found ");
                throw new KeyNotFoundException($"User {id} not found ");
            }

            return new ReadUserDto(
              Id: user.Id,
              Name: user.Name,
              Email: user.Email,
              CreatedAt: user.CreatedAt
          );


        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {

            var user = await _uow.Users.GetUserByEmailAsync(email);

            if (user == null)
            {
                _logger.LogWarning("User {email} not found ", email);
                throw new KeyNotFoundException($"Email {email} not found ");
            }

            return user;
        }

        public async Task<ReadUserDto> AddUserAsync(CreateUserDto dto)
        {

            var user = await _uow.Users.GetUserByEmailAsync(dto.Email);

            if (user != null)
            {
                throw new InvalidOperationException("Email already exists ");
            }

            var userModel = new User
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                CreatedAt = DateTime.UtcNow
            };

            await _uow.Users.AddUserAsync(userModel);

            return new ReadUserDto(
              Id: userModel.Id,
              Name: userModel.Name,
              Email: userModel.Email,
              CreatedAt: userModel.CreatedAt
            );
        }

        public async Task<ReadUserDto> UpdateUserAsync(Guid id, UpdateUserDto dto)
        {

            var user = await _uow.Users.GetUserByIdAsync(id);

            if (user == null)
            {
                throw new KeyNotFoundException($"User {id} not found ");
            }

            user.Name = dto.Name;
            user.Email = dto.Email;
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            await _uow.Users.UpdateUserAsync(user);
            _logger.LogInformation("User updated successfuly");

            return new ReadUserDto(
              Id: user.Id,
              Name: user.Name,
              Email: user.Email,
              CreatedAt: user.CreatedAt
           );
        }
    }
}
