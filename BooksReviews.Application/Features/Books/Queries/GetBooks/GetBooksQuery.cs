using MediatR;
using AutoMapper;
using BooksReviews.Application.Common.Interfaces;
using BooksReviews.Application.Features.Books.DTOs;

namespace BooksReviews.Application.Features.Books.Queries.GetBooks;

public record GetBooksQuery : IRequest<IEnumerable<BookDto>>;

public class GetBooksQueryHandler : IRequestHandler<GetBooksQuery, IEnumerable<BookDto>>
{
    private readonly IBookRepository _bookRepository;
    private readonly IMapper _mapper;

    public GetBooksQueryHandler(IBookRepository bookRepository, IMapper mapper)
    {
        _bookRepository = bookRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<BookDto>> Handle(GetBooksQuery request, CancellationToken cancellationToken)
    {
        var books = await _bookRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<BookDto>>(books);
    }
}
