namespace BooksReviews.Application.Features.Books.DTOs;

public class BookDto
{
    public string Id { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string CoverUrl { get; set; } = string.Empty;
    public double AverageRating { get; set; }
}
