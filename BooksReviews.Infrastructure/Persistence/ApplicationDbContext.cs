using Microsoft.EntityFrameworkCore;
using BooksReviews.Domain.Entities;

namespace BooksReviews.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Book> Books => Set<Book>();
    public DbSet<Review> Reviews => Set<Review>();
    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Additional configuration if needed (already mostly handled by DataAnnotations)
        modelBuilder.Entity<Book>()
            .HasMany(b => b.Reviews)
            .WithOne(r => r.Book)
            .HasForeignKey(r => r.BookId);

        modelBuilder.Entity<User>()
            .HasMany(u => u.Reviews)
            .WithOne(r => r.User)
            .HasForeignKey(r => r.UserId);

        modelBuilder.Entity<Review>()
            .HasOne(r => r.Book)
            .WithMany(b => b.Reviews)
            .HasForeignKey(r => r.BookId)
            .HasForeignKey(r => r.UserId);
                   
    }
}
