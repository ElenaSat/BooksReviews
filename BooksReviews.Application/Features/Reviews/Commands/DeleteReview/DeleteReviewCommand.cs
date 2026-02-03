using MediatR;
using BooksReviews.Application.Common.Interfaces;

namespace BooksReviews.Application.Features.Reviews.Commands.DeleteReview;

public record DeleteReviewCommand(string Id) : IRequest<Unit>;

public class DeleteReviewCommandHandler : IRequestHandler<DeleteReviewCommand, Unit>
{
    private readonly IReviewRepository _reviewRepository;

    public DeleteReviewCommandHandler(IReviewRepository reviewRepository)
    {
        _reviewRepository = reviewRepository;
    }

    public async Task<Unit> Handle(DeleteReviewCommand request, CancellationToken cancellationToken)
    {
        await _reviewRepository.DeleteAsync(request.Id);
        return Unit.Value;
    }
}
