using MediatR;
using BooksReviews.Application.Common.Interfaces;
using BooksReviews.Application.Common.Models;

namespace BooksReviews.Application.Features.Books.Commands.DeleteBook;

public record DeleteBookCommand(string Id) : IRequest<Result>;

public class DeleteBookCommandHandler : IRequestHandler<DeleteBookCommand, Result>
{
    private readonly IBookRepository _bookRepository;

    public DeleteBookCommandHandler(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }

    public async Task<Result> Handle(DeleteBookCommand request, CancellationToken cancellationToken)
    {
        var book = await _bookRepository.GetByIdAsync(request.Id);
        if (book == null)
            return Result.Failure("Not Found");

        await _bookRepository.DeleteAsync(request.Id);
        return Result.Success();
    }
}
