using MediatR;
using AutoMapper;
using BooksReviews.Application.Common.Interfaces;
using BooksReviews.Application.Features.Books.DTOs;
using BooksReviews.Application.Common.Models;

namespace BooksReviews.Application.Features.Books.Queries.GetBookById;

public record GetBookByIdQuery(string Id) : IRequest<Result<BookDto>>;

public class GetBookByIdQueryHandler : IRequestHandler<GetBookByIdQuery, Result<BookDto>>
{
    private readonly IBookRepository _bookRepository;
    private readonly IMapper _mapper;

    public GetBookByIdQueryHandler(IBookRepository bookRepository, IMapper mapper)
    {
        _bookRepository = bookRepository;
        _mapper = mapper;
    }

    public async Task<Result<BookDto>> Handle(GetBookByIdQuery request, CancellationToken cancellationToken)
    {
        var book = await _bookRepository.GetByIdAsync(request.Id);
        
        if (book == null)
            return Result<BookDto>.Failure("Not Found");

        return Result<BookDto>.Success(_mapper.Map<BookDto>(book));
    }
}
