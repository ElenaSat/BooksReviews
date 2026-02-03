using BooksReviews.Domain.Entities;

namespace BooksReviews.Application.Common.Interfaces;

public interface IBookRepository
{
    Task<Book?> GetByIdAsync(string id);
    Task<IEnumerable<Book>> GetAllAsync();
    Task AddAsync(Book book);
    Task UpdateAsync(Book book);
    Task DeleteAsync(string id);
}
