using MediatR;
using AutoMapper;
using BooksReviews.Application.Common.Interfaces;
using BooksReviews.Application.Features.Users.DTOs;
using BooksReviews.Application.Common.Models;

namespace BooksReviews.Application.Features.Users.Queries.GetUserById;

public record GetUserByIdQuery(string Id) : IRequest<Result<UserDto>>;

public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, Result<UserDto>>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public GetUserByIdQueryHandler(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<Result<UserDto>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.Id);
        
        if (user == null)
            return Result<UserDto>.Failure("Not Found");

        return Result<UserDto>.Success(_mapper.Map<UserDto>(user));
    }
}
