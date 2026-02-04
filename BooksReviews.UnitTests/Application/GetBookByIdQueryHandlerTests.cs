using Moq;
using FluentAssertions;
using AutoMapper;
using BooksReviews.Application.Features.Books.Queries.GetBookById;
using BooksReviews.Application.Features.Books.DTOs;
using BooksReviews.Application.Common.Interfaces;
using BooksReviews.Domain.Entities;

namespace BooksReviews.UnitTests.Application;

public class GetBookByIdQueryHandlerTests
{
    private readonly Mock<IBookRepository> _bookRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly GetBookByIdQueryHandler _handler;

    public GetBookByIdQueryHandlerTests()
    {
        _bookRepositoryMock = new Mock<IBookRepository>();
        _mapperMock = new Mock<IMapper>();
        _handler = new GetBookByIdQueryHandler(_bookRepositoryMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_ExistingBook_ReturnsSuccessResult()
    {
        // Arrange
        var bookId = "1";
        var book = new Book { Id = bookId, Title = "Test Book" };
        var bookDto = new BookDto { Id = bookId, Title = "Test Book" };

        _bookRepositoryMock.Setup(x => x.GetByIdAsync(bookId)).ReturnsAsync(book);
        _mapperMock.Setup(x => x.Map<BookDto>(book)).Returns(bookDto);

        var query = new GetBookByIdQuery(bookId);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(bookDto);
    }

    [Fact]
    public async Task Handle_NonExistingBook_ReturnsFailureResult()
    {
        // Arrange
        var bookId = "99";
        _bookRepositoryMock.Setup(x => x.GetByIdAsync(bookId)).ReturnsAsync((Book?)null);

        var query = new GetBookByIdQuery(bookId);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("Not Found");
    }
}
