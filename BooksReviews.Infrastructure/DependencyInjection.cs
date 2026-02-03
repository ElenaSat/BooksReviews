using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using BooksReviews.Application.Common.Interfaces;
using BooksReviews.Infrastructure.Persistence;
using BooksReviews.Infrastructure.Persistence.Repositories;
using BooksReviews.Infrastructure.Authentication;
using Microsoft.Extensions.Configuration;

namespace BooksReviews.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));

        services.AddScoped<IBookRepository, BookRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IReviewRepository, ReviewRepository>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

        return services;
    }
}
