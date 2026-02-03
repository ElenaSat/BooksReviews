using MediatR;
using BooksReviews.Application.Common.Interfaces;
using BooksReviews.Domain.Entities;

namespace BooksReviews.Application.Features.Books.Commands.CreateBook;

public record CreateBookCommand(
    string Id,
    string Title,
    string Author,
    string Category,
    string Description,
    string CoverUrl) : IRequest<string>;

public class CreateBookCommandHandler : IRequestHandler<CreateBookCommand, string>
{
    private readonly IBookRepository _bookRepository;

    public CreateBookCommandHandler(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }

    public async Task<string> Handle(CreateBookCommand request, CancellationToken cancellationToken)
    {
        var book = new Book
        {
            Id = request.Id,
            Title = request.Title,
            Author = request.Author,
            Category = request.Category,
            Description = request.Description,
            CoverUrl = request.CoverUrl
        };

        await _bookRepository.AddAsync(book);

        return book.Id;
    }
}
