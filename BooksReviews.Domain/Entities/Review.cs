using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BooksReviews.Domain.Entities;

[Table("Reviews")]
public class Review
{
    [Key]
    public string Id { get; set; } = string.Empty;

    [Required]
    public string BookId { get; set; } = string.Empty;

    [Required]
    public string UserId { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string UserName { get; set; } = string.Empty;

    [Range(0, 5)]
    public double Rating { get; set; }

    public string Comment { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }

    [ForeignKey("BookId")]
    public virtual Book? Book { get; set; }

    [ForeignKey("UserId")]
    public virtual User? User { get; set; }
}
