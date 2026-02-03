using MediatR;
using AutoMapper;
using BooksReviews.Application.Common.Interfaces;
using BooksReviews.Application.Features.Books.DTOs;

namespace BooksReviews.Application.Features.Books.Queries.GetBookById;

public record GetBookByIdQuery(string Id) : IRequest<BookDto?>;

public class GetBookByIdQueryHandler : IRequestHandler<GetBookByIdQuery, BookDto?>
{
    private readonly IBookRepository _bookRepository;
    private readonly IMapper _mapper;

    public GetBookByIdQueryHandler(IBookRepository bookRepository, IMapper mapper)
    {
        _bookRepository = bookRepository;
        _mapper = mapper;
    }

    public async Task<BookDto?> Handle(GetBookByIdQuery request, CancellationToken cancellationToken)
    {
        var book = await _bookRepository.GetByIdAsync(request.Id);
        return _mapper.Map<BookDto>(book);
    }
}
