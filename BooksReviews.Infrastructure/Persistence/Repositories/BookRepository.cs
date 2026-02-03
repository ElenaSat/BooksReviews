using Microsoft.EntityFrameworkCore;
using BooksReviews.Application.Common.Interfaces;
using BooksReviews.Domain.Entities;

namespace BooksReviews.Infrastructure.Persistence.Repositories;

public class BookRepository : IBookRepository
{
    private readonly ApplicationDbContext _context;

    public BookRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Book?> GetByIdAsync(string id)
    {
        return await _context.Books
            .Include(b => b.Reviews)
            .FirstOrDefaultAsync(b => b.Id == id);
    }

    public async Task<IEnumerable<Book>> GetAllAsync()
    {
        return await _context.Books.ToListAsync();
    }

    public async Task AddAsync(Book book)
    {
        await _context.Books.AddAsync(book);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Book book)
    {
        _context.Books.Update(book);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(string id)
    {
        var book = await _context.Books.FindAsync(id);
        if (book != null)
        {
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
        }
    }
}
