using MediatR;
using AutoMapper;
using BooksReviews.Application.Common.Interfaces;
using BooksReviews.Application.Features.Books.DTOs;
using BooksReviews.Application.Common.Models;

namespace BooksReviews.Application.Features.Books.Queries.GetBooks;

public record GetBooksQuery : IRequest<Result<IEnumerable<BookDto>>>;

public class GetBooksQueryHandler : IRequestHandler<GetBooksQuery, Result<IEnumerable<BookDto>>>
{
    private readonly IBookRepository _bookRepository;
    private readonly IMapper _mapper;

    public GetBooksQueryHandler(IBookRepository bookRepository, IMapper mapper)
    {
        _bookRepository = bookRepository;
        _mapper = mapper;
    }

    public async Task<Result<IEnumerable<BookDto>>> Handle(GetBooksQuery request, CancellationToken cancellationToken)
    {
        var books = await _bookRepository.GetAllAsync();
        return Result<IEnumerable<BookDto>>.Success(_mapper.Map<IEnumerable<BookDto>>(books));
    }
}
