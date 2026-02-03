using MediatR;
using BooksReviews.Application.Common.Interfaces;
using BooksReviews.Domain.Entities;

namespace BooksReviews.Application.Features.Users.Commands.CreateUser;

public record CreateUserCommand(
    string Id,
    string Name,
    string Email,
    string AvatarUrl) : IRequest<string>;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, string>
{
    private readonly IUserRepository _userRepository;

    public CreateUserCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<string> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var user = new User
        {
            Id = request.Id,
            Name = request.Name,
            Email = request.Email,
            AvatarUrl = request.AvatarUrl
        };

        await _userRepository.AddAsync(user);

        return user.Id;
    }
}
