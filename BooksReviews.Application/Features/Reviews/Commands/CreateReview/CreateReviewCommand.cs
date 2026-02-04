using MediatR;
using BooksReviews.Application.Common.Interfaces;
using BooksReviews.Application.Common.Models;
using BooksReviews.Domain.Entities;

namespace BooksReviews.Application.Features.Reviews.Commands.CreateReview;

public record CreateReviewCommand(
    string Id,
    string BookId,
    string UserId,
    string UserName,
    double Rating,
    string Comment) : IRequest<Result<string>>;

public class CreateReviewCommandHandler : IRequestHandler<CreateReviewCommand, Result<string>>
{
    private readonly IReviewRepository _reviewRepository;

    public CreateReviewCommandHandler(IReviewRepository reviewRepository)
    {
        _reviewRepository = reviewRepository;
    }

    public async Task<Result<string>> Handle(CreateReviewCommand request, CancellationToken cancellationToken)
    {
        var review = new Review
        {
            Id = request.Id,
            BookId = request.BookId,
            UserId = request.UserId,
            UserName = request.UserName,
            Rating = request.Rating,
            Comment = request.Comment,
            CreatedAt = DateTime.UtcNow
        };

        await _reviewRepository.AddAsync(review);

        return Result<string>.Success(review.Id);
    }
}
