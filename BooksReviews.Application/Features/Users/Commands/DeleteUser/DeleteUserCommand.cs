using MediatR;
using BooksReviews.Application.Common.Interfaces;
using BooksReviews.Application.Common.Models;

namespace BooksReviews.Application.Features.Users.Commands.DeleteUser;

public record DeleteUserCommand(string Id) : IRequest<Result>;

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, Result>
{
    private readonly IUserRepository _userRepository;

    public DeleteUserCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.Id);
        if (user == null)
            return Result.Failure("Not Found");

        await _userRepository.DeleteAsync(request.Id);
        return Result.Success();
    }
}
