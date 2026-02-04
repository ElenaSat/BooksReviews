using Moq;
using FluentAssertions;
using AutoMapper;
using BooksReviews.Application.Features.Users.Commands.CreateUser;
using BooksReviews.Application.Features.Users.Commands.UpdateUser;
using BooksReviews.Application.Features.Users.Commands.DeleteUser;
using BooksReviews.Application.Features.Users.Commands.Login;
using BooksReviews.Application.Features.Users.Queries.GetUsers;
using BooksReviews.Application.Features.Users.Queries.GetUserById;
using BooksReviews.Application.Features.Users.DTOs;
using BooksReviews.Application.Common.Interfaces;
using BooksReviews.Domain.Entities;
using BooksReviews.Application.Common.Models;

namespace BooksReviews.UnitTests.Application;

public class UserTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IPasswordHasher> _passwordHasherMock;
    private readonly Mock<IJwtTokenGenerator> _jwtTokenGeneratorMock;
    private readonly Mock<IMapper> _mapperMock;

    public UserTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _passwordHasherMock = new Mock<IPasswordHasher>();
        _jwtTokenGeneratorMock = new Mock<IJwtTokenGenerator>();
        _mapperMock = new Mock<IMapper>();
    }

    [Fact]
    public async Task CreateUserCommandHandler_ValidCommand_ReturnsSuccess()
    {
        // Arrange
        var command = new CreateUserCommand("1", "Name", "email@test.com", "URL", "password");
        _passwordHasherMock.Setup(h => h.HashPassword("password")).Returns("hashed");
        _userRepositoryMock.Setup(r => r.AddAsync(It.IsAny<User>())).Returns(Task.CompletedTask);

        var handler = new CreateUserCommandHandler(_userRepositoryMock.Object, _passwordHasherMock.Object);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be("1");
        _userRepositoryMock.Verify(r => r.AddAsync(It.IsAny<User>()), Times.Once);
    }

    [Fact]
    public async Task LoginCommandHandler_ValidCredentials_ReturnsSuccess()
    {
        // Arrange
        var command = new LoginCommand("email@test.com", "password");
        var user = new User { Id = "1", Email = "email@test.com", PasswordHash = "hashed" };
        _userRepositoryMock.Setup(r => r.GetByEmailAsync(command.Email)).ReturnsAsync(user);
        _passwordHasherMock.Setup(h => h.VerifyPassword("password", "hashed")).Returns(true);
        _jwtTokenGeneratorMock.Setup(g => g.GenerateToken(user)).Returns("token");

        var handler = new LoginCommandHandler(_userRepositoryMock.Object, _passwordHasherMock.Object, _jwtTokenGeneratorMock.Object);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Token.Should().Be("token");
    }

    [Fact]
    public async Task LoginCommandHandler_InvalidCredentials_ReturnsFailure()
    {
        // Arrange
        var command = new LoginCommand("email@test.com", "wrong");
        var user = new User { Id = "1", Email = "email@test.com", PasswordHash = "hashed" };
        _userRepositoryMock.Setup(r => r.GetByEmailAsync(command.Email)).ReturnsAsync(user);
        _passwordHasherMock.Setup(h => h.VerifyPassword("wrong", "hashed")).Returns(false);

        var handler = new LoginCommandHandler(_userRepositoryMock.Object, _passwordHasherMock.Object, _jwtTokenGeneratorMock.Object);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("Invalid email or password");
    }

    [Fact]
    public async Task UpdateUserCommandHandler_ExistingUser_ReturnsSuccess()
    {
        // Arrange
        var userId = "1";
        var user = new User { Id = userId, Name = "Old" };
        var command = new UpdateUserCommand(userId, "New", "email@test.com", "URL");
        _userRepositoryMock.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync(user);
        _userRepositoryMock.Setup(r => r.UpdateAsync(user)).Returns(Task.CompletedTask);

        var handler = new UpdateUserCommandHandler(_userRepositoryMock.Object);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        user.Name.Should().Be("New");
    }

    [Fact]
    public async Task GetAllUsersQueryHandler_ReturnsUsers()
    {
        // Arrange
        var users = new List<User> { new User { Id = "1" } };
        _userRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(users);
        _mapperMock.Setup(m => m.Map<IEnumerable<UserDto>>(users)).Returns(new List<UserDto> { new UserDto { Id = "1" } });

        var handler = new GetUsersQueryHandler(_userRepositoryMock.Object, _mapperMock.Object);

        // Act
        var result = await handler.Handle(new GetUsersQuery(), CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().HaveCount(1);
    }
}
