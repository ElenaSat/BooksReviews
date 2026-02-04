using MediatR;
using BooksReviews.Application.Common.Interfaces;
using BooksReviews.Application.Common.Models;
using BooksReviews.Domain.Entities;

namespace BooksReviews.Application.Features.Users.Commands.CreateUser;

public record CreateUserCommand(
    string Id,
    string Name,
    string Email,
    string AvatarUrl,
    string Password) : IRequest<Result<string>>;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Result<string>>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;

    public CreateUserCommandHandler(IUserRepository userRepository, IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<Result<string>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var user = new User
        {
            Id = request.Id,
            Name = request.Name,
            Email = request.Email,
            AvatarUrl = request.AvatarUrl,
            PasswordHash = _passwordHasher.HashPassword(request.Password)
        };

        await _userRepository.AddAsync(user);

        return Result<string>.Success(user.Id);
    }
}
