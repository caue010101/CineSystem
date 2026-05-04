using CineList.Application.Dtos;
using CineList.Domain.Entities;

namespace CineList.Application.Interfaces
{

    public interface IUserService
    {
        Task<ReadUserDto?> GetUserByIdAsync(Guid id);
        Task<User?> GetUserByEmailAsync(string email);
        Task<ReadUserDto> AddUserAsync(CreateUserDto dto);
        Task<ReadUserDto> UpdateUserAsync(Guid id, UpdateUserDto dto);
    }
}
