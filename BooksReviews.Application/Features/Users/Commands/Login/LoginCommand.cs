using MediatR;
using BooksReviews.Application.Common.Interfaces;
using BooksReviews.Application.Common.Models;

namespace BooksReviews.Application.Features.Users.Commands.Login;

public record LoginCommand(string Email, string Password) : IRequest<Result<LoginResponse>>;

public record LoginResponse(string Id, string Name, string Email, string AvatarUrl, string Token);

public class LoginCommandHandler : IRequestHandler<LoginCommand, Result<LoginResponse>>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public LoginCommandHandler(IUserRepository userRepository, IPasswordHasher passwordHasher, IJwtTokenGenerator jwtTokenGenerator)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<Result<LoginResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email);
        
        if (user == null || !_passwordHasher.VerifyPassword(request.Password, user.PasswordHash))
        {
            return Result<LoginResponse>.Failure("Invalid email or password");
        }

        var token = _jwtTokenGenerator.GenerateToken(user);

        return Result<LoginResponse>.Success(new LoginResponse(user.Id, user.Name, user.Email, user.AvatarUrl, token));
    }
}
