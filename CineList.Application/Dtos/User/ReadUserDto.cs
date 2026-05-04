

namespace CineList.Application.Dtos
{

    public record ReadUserDto(
        Guid Id,
        string Name,
        string Email,
        DateTime CreatedAt
    );
}
