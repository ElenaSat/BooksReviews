using MediatR;
using BooksReviews.Application.Common.Interfaces;
using BooksReviews.Application.Common.Models;

namespace BooksReviews.Application.Features.Reviews.Commands.DeleteReview;

public record DeleteReviewCommand(string Id) : IRequest<Result>;

public class DeleteReviewCommandHandler : IRequestHandler<DeleteReviewCommand, Result>
{
    private readonly IReviewRepository _reviewRepository;

    public DeleteReviewCommandHandler(IReviewRepository reviewRepository)
    {
        _reviewRepository = reviewRepository;
    }

    public async Task<Result> Handle(DeleteReviewCommand request, CancellationToken cancellationToken)
    {
        var review = await _reviewRepository.GetByIdAsync(request.Id);
        if (review == null)
            return Result.Failure("Not Found");

        await _reviewRepository.DeleteAsync(request.Id);
        return Result.Success();
    }
}
