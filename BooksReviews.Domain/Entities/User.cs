using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BooksReviews.Domain.Entities;

[Table("Users")]
public class User
{
    [Key]
    public string Id { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [StringLength(150)]
    public string Email { get; set; } = string.Empty;

    public string AvatarUrl { get; set; } = string.Empty;

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
}
