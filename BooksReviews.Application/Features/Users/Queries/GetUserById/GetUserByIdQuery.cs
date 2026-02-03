using MediatR;
using AutoMapper;
using BooksReviews.Application.Common.Interfaces;
using BooksReviews.Application.Features.Users.DTOs;

namespace BooksReviews.Application.Features.Users.Queries.GetUserById;

public record GetUserByIdQuery(string Id) : IRequest<UserDto?>;

public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserDto?>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public GetUserByIdQueryHandler(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<UserDto?> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.Id);
        return _mapper.Map<UserDto>(user);
    }
}
