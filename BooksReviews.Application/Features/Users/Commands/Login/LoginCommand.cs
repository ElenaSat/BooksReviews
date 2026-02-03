using MediatR;
using BooksReviews.Application.Common.Interfaces;

namespace BooksReviews.Application.Features.Users.Commands.Login;

public record LoginCommand(string Email, string Password) : IRequest<LoginResponse?>;

public record LoginResponse(string Id, string Name, string Email, string AvatarUrl, string Token);

public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponse?>
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

    public async Task<LoginResponse?> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email);
        
        if (user == null || !_passwordHasher.VerifyPassword(request.Password, user.PasswordHash))
        {
            return null;
        }

        var token = _jwtTokenGenerator.GenerateToken(user);

        return new LoginResponse(user.Id, user.Name, user.Email, user.AvatarUrl, token);
    }
}
