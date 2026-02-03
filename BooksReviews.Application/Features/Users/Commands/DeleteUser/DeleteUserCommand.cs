using MediatR;
using BooksReviews.Application.Common.Interfaces;

namespace BooksReviews.Application.Features.Users.Commands.DeleteUser;

public record DeleteUserCommand(string Id) : IRequest<Unit>;

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, Unit>
{
    private readonly IUserRepository _userRepository;

    public DeleteUserCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        await _userRepository.DeleteAsync(request.Id);
        return Unit.Value;
    }
}
