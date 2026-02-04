using MediatR;
using AutoMapper;
using BooksReviews.Application.Common.Interfaces;
using BooksReviews.Application.Features.Reviews.DTOs;
using BooksReviews.Application.Common.Models;

namespace BooksReviews.Application.Features.Reviews.Queries.GetReviewsByBookId;

public record GetReviewsByBookIdQuery(string BookId) : IRequest<Result<IEnumerable<ReviewDto>>>;

public class GetReviewsByBookIdQueryHandler : IRequestHandler<GetReviewsByBookIdQuery, Result<IEnumerable<ReviewDto>>>
{
    private readonly IReviewRepository _reviewRepository;
    private readonly IMapper _mapper;

    public GetReviewsByBookIdQueryHandler(IReviewRepository reviewRepository, IMapper mapper)
    {
        _reviewRepository = reviewRepository;
        _mapper = mapper;
    }

    public async Task<Result<IEnumerable<ReviewDto>>> Handle(GetReviewsByBookIdQuery request, CancellationToken cancellationToken)
    {
        var reviews = await _reviewRepository.GetByBookIdAsync(request.BookId);
        return Result<IEnumerable<ReviewDto>>.Success(_mapper.Map<IEnumerable<ReviewDto>>(reviews));
    }
}
