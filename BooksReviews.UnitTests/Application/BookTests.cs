using Moq;
using FluentAssertions;
using AutoMapper;
using BooksReviews.Application.Features.Books.Commands.CreateBook;
using BooksReviews.Application.Features.Books.Commands.UpdateBook;
using BooksReviews.Application.Features.Books.Commands.DeleteBook;
using BooksReviews.Application.Features.Books.Queries.GetBooks;
using BooksReviews.Application.Features.Books.DTOs;
using BooksReviews.Application.Common.Interfaces;
using BooksReviews.Domain.Entities;
using BooksReviews.Application.Common.Models;

namespace BooksReviews.UnitTests.Application;

public class BookTests
{
    private readonly Mock<IBookRepository> _bookRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;

    public BookTests()
    {
        _bookRepositoryMock = new Mock<IBookRepository>();
        _mapperMock = new Mock<IMapper>();
    }

    [Fact]
    public async Task CreateBookCommandHandler_ValidCommand_ReturnsSuccess()
    {
        // Arrange
        var command = new CreateBookCommand("1", "Title", "Author", "Category", "Desc", "URL");
        var book = new Book { Id = "1" };
        _mapperMock.Setup(m => m.Map<Book>(command)).Returns(book);
        _bookRepositoryMock.Setup(r => r.AddAsync(book)).Returns(Task.CompletedTask);

        var handler = new CreateBookCommandHandler(_bookRepositoryMock.Object, _mapperMock.Object);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be("1");
        _bookRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Book>()), Times.Once);
    }

    [Fact]
    public async Task UpdateBookCommandHandler_ExistingBook_ReturnsSuccess()
    {
        // Arrange
        var bookId = "1";
        var command = new UpdateBookCommand(bookId, "New Title", "Author", "Cat", "Desc", "URL");
        var book = new Book { Id = bookId, Title = "Old Title" };
        _bookRepositoryMock.Setup(r => r.GetByIdAsync(bookId)).ReturnsAsync(book);
        _bookRepositoryMock.Setup(r => r.UpdateAsync(book)).Returns(Task.CompletedTask);

        var handler = new UpdateBookCommandHandler(_bookRepositoryMock.Object);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        book.Title.Should().Be("New Title");
        _bookRepositoryMock.Verify(r => r.UpdateAsync(book), Times.Once);
    }

    [Fact]
    public async Task DeleteBookCommandHandler_ExistingBook_ReturnsSuccess()
    {
        // Arrange
        var bookId = "1";
        var book = new Book { Id = bookId };
        _bookRepositoryMock.Setup(r => r.GetByIdAsync(bookId)).ReturnsAsync(book);
        _bookRepositoryMock.Setup(r => r.DeleteAsync(bookId)).Returns(Task.CompletedTask);

        var handler = new DeleteBookCommandHandler(_bookRepositoryMock.Object);

        // Act
        var result = await handler.Handle(new DeleteBookCommand(bookId), CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        _bookRepositoryMock.Verify(r => r.DeleteAsync(bookId), Times.Once);
    }

    [Fact]
    public async Task GetBooksQueryHandler_ReturnsListOfBooks()
    {
        // Arrange
        var books = new List<Book> { new Book { Id = "1" } };
        var bookDtos = new List<BookDto> { new BookDto { Id = "1" } };
        _bookRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(books);
        _mapperMock.Setup(m => m.Map<IEnumerable<BookDto>>(books)).Returns(bookDtos);

        var handler = new GetBooksQueryHandler(_bookRepositoryMock.Object, _mapperMock.Object);

        // Act
        var result = await handler.Handle(new GetBooksQuery(), CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().HaveCount(1);
    }
}
