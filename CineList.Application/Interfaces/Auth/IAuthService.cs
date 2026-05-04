using CineList.Application.Dtos;

namespace CineList.Application.Interfaces.Auth
{

    public interface IAuthService
    {

        Task<string?> AuthUserAsync(LoginUserDto dto);
    }
}
