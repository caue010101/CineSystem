

namespace CineList.Application.Dtos
{

    public record UpdateUserDto(
        string Name,
        string Email,
        string Password
    );
}
