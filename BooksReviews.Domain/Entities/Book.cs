using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BooksReviews.Domain.Entities;

[Table("Books")]
public class Book
{
    [Key]
    public string Id { get; set; } = string.Empty;

    [Required]
    [StringLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string Author { get; set; } = string.Empty;

    [StringLength(50)]
    public string Category { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string CoverUrl { get; set; } = string.Empty;

    public double AverageRating { get; set; }

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
}
