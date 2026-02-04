using MediatR;
using AutoMapper;
using BooksReviews.Application.Common.Interfaces;
using BooksReviews.Application.Features.Users.DTOs;
using BooksReviews.Application.Common.Models;

namespace BooksReviews.Application.Features.Users.Queries.GetUsers;

public record GetUsersQuery : IRequest<Result<IEnumerable<UserDto>>>;

public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, Result<IEnumerable<UserDto>>>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public GetUsersQueryHandler(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<Result<IEnumerable<UserDto>>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        var users = await _userRepository.GetAllAsync();
        return Result<IEnumerable<UserDto>>.Success(_mapper.Map<IEnumerable<UserDto>>(users));
    }
}
