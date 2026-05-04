using Moq;
using FluentAssertions;
using CineList.Application.Services;
using CineList.Domain.Entities;
using CineList.Domain.Interfaces;
using CineList.Application.Dtos;
using Microsoft.Extensions.Logging;


namespace CineList.Tests.Services
{

    public class UserServicesTests
    {
        private readonly Mock<IUnitOfWork> _uowMock;
        private readonly Mock<ILogger<UserService>> _loggerMock;
        private readonly Mock<IUserRepository> _userRepoMock;
        private readonly UserService _userService;

        public UserServicesTests()
        {
            _uowMock = new Mock<IUnitOfWork>();
            _loggerMock = new Mock<ILogger<UserService>>();
            _userRepoMock = new Mock<IUserRepository>();


            _uowMock.Setup(u => u.Users).Returns(_userRepoMock.Object);

            _userService = new UserService(_uowMock.Object, _loggerMock.Object);
        }

        [Fact]

        public async Task GetUserByIdAsync_ShouldReturnReadUserDto_WhenUserExists()
        {

            var id = Guid.NewGuid();

            var userExists = new User
            {
                Id = id,
                Name = "goja22",
                Email = "goja22@gmail.com",
                PasswordHash = "fuba2020",
                CreatedAt = DateTime.UtcNow
            };

            _userRepoMock
              .Setup(r => r.GetUserByIdAsync(id))
              .ReturnsAsync(userExists);

            var result = await _userService.GetUserByIdAsync(id);

            result.Should().NotBeNull();
            result.Name.Should().Be("goja22");
            result.Email.Should().Be("goja22@gmail.com");

        }

        [Fact]

        public async Task GetUserByIdAsync_ShouldThrowKeyNotFoundException_WhenUserNotExists()
        {

            var id = Guid.NewGuid();

            _userRepoMock
              .Setup(r => r.GetUserByIdAsync(id))
              .ReturnsAsync((User?)null);

            var act = async () => await _userService.GetUserByIdAsync(id);

            await act.Should().ThrowAsync<KeyNotFoundException>()
              .WithMessage($"User {id} not found ");

        }

        [Fact]

        public async Task AddUserAsync_ShouldReturnReadUserDto_WhenUserIsCreated()
        {
            var dto = new CreateUserDto("joao", "joao22@gmail.com", "fuba2015");

            _userRepoMock
              .Setup(r => r.GetUserByEmailAsync(dto.Email))
              .ReturnsAsync((User?)null);

            _userRepoMock
              .Setup(r => r.AddUserAsync(It.IsAny<User>()))
              .Returns(Task.CompletedTask);

            var result = await _userService.AddUserAsync(dto);

            result.Should().NotBeNull();
            result.Name.Should().Be("joao");
            result.Email.Should().Be("joao22@gmail.com");
        }

        [Fact]

        public async Task AddUserAsync_ShouldThrowInvalidOperationException_WhenEmailReadyExists()
        {

            var dto = new CreateUserDto("joao", "joao22@gmail.com", "fuba2015");

            _userRepoMock
              .Setup(r => r.GetUserByEmailAsync(dto.Email))
              .ReturnsAsync(new User { Email = dto.Email });

            var act = async () => await _userService.AddUserAsync(dto);

            await act.Should().ThrowAsync<InvalidOperationException>()
              .WithMessage("Email already exists ");
        }

        [Fact]

        public async Task UpdateUserAsync_ShouldReturnReadUserDto_WhenUserExists()
        {

            var id = Guid.NewGuid();
            var dto = new UpdateUserDto("goja", "goja23@gmail.com", "fuba2020");

            var existsUser = new User
            {
                Id = id,
                Name = "joao",
                Email = "joao22@gmail.com",
                PasswordHash = "fuba2015",
                CreatedAt = DateTime.UtcNow
            };

            _userRepoMock
              .Setup(r => r.GetUserByIdAsync(id))
              .ReturnsAsync(existsUser);

            _userRepoMock
              .Setup(r => r.UpdateUserAsync(It.IsAny<User>()))
              .Returns(Task.CompletedTask);

            var result = await _userService.UpdateUserAsync(id, dto);

            result.Should().NotBeNull();
            result.Name.Should().Be("goja");
            result.Email.Should().Be("goja23@gmail.com");
        }

        [Fact]

        public async Task UpdateUserAsync_ShouldThrowKeyNotFoundException_WhenUserNotFound()
        {

            var id = Guid.NewGuid();
            var dto = new UpdateUserDto("goja", "goja23@gmail.com", "fuba2020");

            _userRepoMock
              .Setup(r => r.GetUserByIdAsync(id))
              .ReturnsAsync((User?)null);

            var act = async () => await _userService.UpdateUserAsync(id, dto);

            await act.Should().ThrowAsync<KeyNotFoundException>()
              .WithoutMessage("User not found ");
        }
    }
}
