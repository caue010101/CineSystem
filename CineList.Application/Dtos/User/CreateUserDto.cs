

namespace CineList.Application.Dtos
{

    public record CreateUserDto(
        string Name,
        string Email,
        string Password
    );
}
