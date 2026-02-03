using MediatR;
using BooksReviews.Application.Common.Interfaces;
using BooksReviews.Domain.Entities;

namespace BooksReviews.Application.Features.Books.Commands.UpdateBook;

public record UpdateBookCommand(
    string Id,
    string Title,
    string Author,
    string Category,
    string Description,
    string CoverUrl) : IRequest<Unit>;

public class UpdateBookCommandHandler : IRequestHandler<UpdateBookCommand, Unit>
{
    private readonly IBookRepository _bookRepository;

    public UpdateBookCommandHandler(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }

    public async Task<Unit> Handle(UpdateBookCommand request, CancellationToken cancellationToken)
    {
        var book = await _bookRepository.GetByIdAsync(request.Id);
        
        if (book == null) return Unit.Value;

        book.Title = request.Title;
        book.Author = request.Author;
        book.Category = request.Category;
        book.Description = request.Description;
        book.CoverUrl = request.CoverUrl;

        await _bookRepository.UpdateAsync(book);

        return Unit.Value;
    }
}
