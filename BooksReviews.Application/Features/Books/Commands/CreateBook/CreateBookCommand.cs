using MediatR;
using BooksReviews.Application.Common.Interfaces;
using BooksReviews.Domain.Entities;
using BooksReviews.Application.Common.Models;
using AutoMapper;

namespace BooksReviews.Application.Features.Books.Commands.CreateBook;

public record CreateBookCommand(
    string Id,
    string Title,
    string Author,
    string Category,
    string Description,
    string CoverUrl) : IRequest<Result<string>>;

public class CreateBookCommandHandler : IRequestHandler<CreateBookCommand, Result<string>>
{
    private readonly IBookRepository _bookRepository;
    private readonly IMapper _mapper;

    public CreateBookCommandHandler(IBookRepository bookRepository, IMapper mapper)
    {
        _bookRepository = bookRepository;
        _mapper = mapper;
    }

    public async Task<Result<string>> Handle(CreateBookCommand request, CancellationToken cancellationToken)
    {
        var book = _mapper.Map<Book>(request);

        await _bookRepository.AddAsync(book);

        return Result<string>.Success(book.Id);
    }
}
