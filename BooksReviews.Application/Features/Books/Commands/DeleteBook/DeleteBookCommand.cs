using MediatR;
using BooksReviews.Application.Common.Interfaces;

namespace BooksReviews.Application.Features.Books.Commands.DeleteBook;

public record DeleteBookCommand(string Id) : IRequest<Unit>;

public class DeleteBookCommandHandler : IRequestHandler<DeleteBookCommand, Unit>
{
    private readonly IBookRepository _bookRepository;

    public DeleteBookCommandHandler(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }

    public async Task<Unit> Handle(DeleteBookCommand request, CancellationToken cancellationToken)
    {
        await _bookRepository.DeleteAsync(request.Id);
        return Unit.Value;
    }
}
