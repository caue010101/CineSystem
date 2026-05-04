using CineList.Application.Interfaces.Auth;
using CineList.Application.Interfaces.Jwt;
using CineList.Application.Dtos;
using Microsoft.Extensions.Logging;
using CineList.Domain.Interfaces;

namespace CineList.Application.Services
{

    public class AuthService : IAuthService
    {

        private readonly IJwtService _jwtService;
        private readonly IUnitOfWork _uow;
        private readonly ILogger<AuthService> _logger;

        public AuthService(IJwtService jwtService, ILogger<AuthService> logger, IUnitOfWork uow)
        {
            this._jwtService = jwtService;
            this._logger = logger;
            this._uow = uow;
        }

        public async Task<string?> AuthUserAsync(LoginUserDto dto)
        {

            var user = await _uow.Users.GetUserByEmailAsync(dto.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            {
                _logger.LogWarning("Invalid Email or password ");
                throw new UnauthorizedAccessException("Invalid credentials");
            }

            return _jwtService.GenerateToken(user);

        }
    }
}
