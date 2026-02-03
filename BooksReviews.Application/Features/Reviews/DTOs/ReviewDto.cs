namespace BooksReviews.Application.Features.Reviews.DTOs;

public class ReviewDto
{
    public string Id { get; set; } = string.Empty;
    public string BookId { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public double Rating { get; set; }
    public string Comment { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}
