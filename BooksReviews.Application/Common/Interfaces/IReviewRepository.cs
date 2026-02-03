using BooksReviews.Domain.Entities;

namespace BooksReviews.Application.Common.Interfaces;

public interface IReviewRepository
{
    Task<Review?> GetByIdAsync(string id);
    Task<IEnumerable<Review>> GetByBookIdAsync(string bookId);
    Task<IEnumerable<Review>> GetByUserIdAsync(string userId);
    Task AddAsync(Review review);
    Task UpdateAsync(Review review);
    Task DeleteAsync(string id);
}
