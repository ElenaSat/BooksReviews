using MediatR;
using BooksReviews.Application.Common.Interfaces;
using BooksReviews.Application.Common.Models;

namespace BooksReviews.Application.Features.Users.Commands.UpdateUser;

public record UpdateUserCommand(
    string Id,
    string Name,
    string Email,
    string AvatarUrl) : IRequest<Result>;

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Result>
{
    private readonly IUserRepository _userRepository;

    public UpdateUserCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.Id);

        if (user == null) 
            return Result.Failure("Not Found");

        user.Name = request.Name;
        user.Email = request.Email;
        user.AvatarUrl = request.AvatarUrl;

        await _userRepository.UpdateAsync(user);

        return Result.Success();
    }
}
