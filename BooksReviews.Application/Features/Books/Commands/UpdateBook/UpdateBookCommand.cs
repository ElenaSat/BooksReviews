using MediatR;
using BooksReviews.Application.Common.Interfaces;
using BooksReviews.Application.Common.Models;

namespace BooksReviews.Application.Features.Books.Commands.UpdateBook;

public record UpdateBookCommand(
    string Id,
    string Title,
    string Author,
    string Category,
    string Description,
    string CoverUrl) : IRequest<Result>;

public class UpdateBookCommandHandler : IRequestHandler<UpdateBookCommand, Result>
{
    private readonly IBookRepository _bookRepository;

    public UpdateBookCommandHandler(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }

    public async Task<Result> Handle(UpdateBookCommand request, CancellationToken cancellationToken)
    {
        var book = await _bookRepository.GetByIdAsync(request.Id);
        
        if (book == null) 
            return Result.Failure("Not Found");

        book.Title = request.Title;
        book.Author = request.Author;
        book.Category = request.Category;
        book.Description = request.Description;
        book.CoverUrl = request.CoverUrl;

        await _bookRepository.UpdateAsync(book);

        return Result.Success();
    }
}
