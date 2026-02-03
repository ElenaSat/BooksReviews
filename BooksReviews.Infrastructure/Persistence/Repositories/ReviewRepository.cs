using Microsoft.EntityFrameworkCore;
using BooksReviews.Application.Common.Interfaces;
using BooksReviews.Domain.Entities;

namespace BooksReviews.Infrastructure.Persistence.Repositories;

public class ReviewRepository : IReviewRepository
{
    private readonly ApplicationDbContext _context;

    public ReviewRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Review?> GetByIdAsync(string id)
    {
        return await _context.Reviews.FindAsync(id);
    }

    public async Task<IEnumerable<Review>> GetByBookIdAsync(string bookId)
    {
        return await _context.Reviews.Where(r => r.BookId == bookId).ToListAsync();
    }

    public async Task<IEnumerable<Review>> GetByUserIdAsync(string userId)
    {
        return await _context.Reviews.Where(r => r.UserId == userId).ToListAsync();
    }

    public async Task AddAsync(Review review)
    {
        await _context.Reviews.AddAsync(review);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Review review)
    {
        _context.Reviews.Update(review);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(string id)
    {
        var review = await _context.Reviews.FindAsync(id);
        if (review != null)
        {
            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();
        }
    }
}
