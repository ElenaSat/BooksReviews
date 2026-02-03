using MediatR;
using AutoMapper;
using BooksReviews.Application.Common.Interfaces;
using BooksReviews.Application.Features.Reviews.DTOs;

namespace BooksReviews.Application.Features.Reviews.Queries.GetReviewsByBookId;

public record GetReviewsByBookIdQuery(string BookId) : IRequest<IEnumerable<ReviewDto>>;

public class GetReviewsByBookIdQueryHandler : IRequestHandler<GetReviewsByBookIdQuery, IEnumerable<ReviewDto>>
{
    private readonly IReviewRepository _reviewRepository;
    private readonly IMapper _mapper;

    public GetReviewsByBookIdQueryHandler(IReviewRepository reviewRepository, IMapper mapper)
    {
        _reviewRepository = reviewRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ReviewDto>> Handle(GetReviewsByBookIdQuery request, CancellationToken cancellationToken)
    {
        var reviews = await _reviewRepository.GetByBookIdAsync(request.BookId);
        return _mapper.Map<IEnumerable<ReviewDto>>(reviews);
    }
}
