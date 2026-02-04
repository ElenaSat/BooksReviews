using Moq;
using FluentAssertions;
using AutoMapper;
using BooksReviews.Application.Features.Reviews.Commands.CreateReview;
using BooksReviews.Application.Features.Reviews.Commands.DeleteReview;
using BooksReviews.Application.Features.Reviews.Queries.GetReviewsByBookId;
using BooksReviews.Application.Features.Reviews.DTOs;
using BooksReviews.Application.Common.Interfaces;
using BooksReviews.Domain.Entities;
using BooksReviews.Application.Common.Models;

namespace BooksReviews.UnitTests.Application;

public class ReviewTests
{
    private readonly Mock<IReviewRepository> _reviewRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;

    public ReviewTests()
    {
        _reviewRepositoryMock = new Mock<IReviewRepository>();
        _mapperMock = new Mock<IMapper>();
    }

    [Fact]
    public async Task CreateReviewCommandHandler_ValidCommand_ReturnsSuccess()
    {
        // Arrange
        var command = new CreateReviewCommand("1", "Book1", "User1", "Name", 5, "Good");
        _reviewRepositoryMock.Setup(r => r.AddAsync(It.IsAny<Review>())).Returns(Task.CompletedTask);

        var handler = new CreateReviewCommandHandler(_reviewRepositoryMock.Object);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be("1");
        _reviewRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Review>()), Times.Once);
    }

    [Fact]
    public async Task DeleteReviewCommandHandler_ExistingReview_ReturnsSuccess()
    {
        // Arrange
        var reviewId = "1";
        var review = new Review { Id = reviewId };
        _reviewRepositoryMock.Setup(r => r.GetByIdAsync(reviewId)).ReturnsAsync(review);
        _reviewRepositoryMock.Setup(r => r.DeleteAsync(reviewId)).Returns(Task.CompletedTask);

        var handler = new DeleteReviewCommandHandler(_reviewRepositoryMock.Object);

        // Act
        var result = await handler.Handle(new DeleteReviewCommand(reviewId), CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        _reviewRepositoryMock.Verify(r => r.DeleteAsync(reviewId), Times.Once);
    }

    [Fact]
    public async Task GetReviewsByBookIdQueryHandler_ReturnsReviews()
    {
        // Arrange
        var bookId = "Book1";
        var reviews = new List<Review> { new Review { Id = "1", BookId = bookId } };
        var reviewDtos = new List<ReviewDto> { new ReviewDto { Id = "1" } };
        _reviewRepositoryMock.Setup(r => r.GetByBookIdAsync(bookId)).ReturnsAsync(reviews);
        _mapperMock.Setup(m => m.Map<IEnumerable<ReviewDto>>(reviews)).Returns(reviewDtos);

        var handler = new GetReviewsByBookIdQueryHandler(_reviewRepositoryMock.Object, _mapperMock.Object);

        // Act
        var result = await handler.Handle(new GetReviewsByBookIdQuery(bookId), CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().HaveCount(1);
    }
}
