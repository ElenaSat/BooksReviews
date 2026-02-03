using BooksReviews.Domain.Entities;

namespace BooksReviews.Application.Common.Interfaces;

public interface IJwtTokenGenerator
{
    string GenerateToken(User user);
}
