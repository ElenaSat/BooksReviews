using MediatR;
using BooksReviews.Application.Common.Interfaces;

namespace BooksReviews.Application.Features.Users.Commands.UpdateUser;

public record UpdateUserCommand(
    string Id,
    string Name,
    string Email,
    string AvatarUrl) : IRequest<Unit>;

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Unit>
{
    private readonly IUserRepository _userRepository;

    public UpdateUserCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Unit> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.Id);

        if (user == null) return Unit.Value;

        user.Name = request.Name;
        user.Email = request.Email;
        user.AvatarUrl = request.AvatarUrl;

        await _userRepository.UpdateAsync(user);

        return Unit.Value;
    }
}
