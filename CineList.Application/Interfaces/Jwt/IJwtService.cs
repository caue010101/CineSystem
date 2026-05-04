using CineList.Domain.Entities;

namespace CineList.Application.Interfaces.Jwt
{

    public interface IJwtService
    {
        string GenerateToken(User user);
    }
}
