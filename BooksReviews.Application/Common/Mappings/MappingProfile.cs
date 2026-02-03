using AutoMapper;
using BooksReviews.Domain.Entities;
using BooksReviews.Application.Features.Books.DTOs;
using BooksReviews.Application.Features.Users.DTOs;
using BooksReviews.Application.Features.Reviews.DTOs;

namespace BooksReviews.Application.Common.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Book, BookDto>().ReverseMap();
        CreateMap<User, UserDto>().ReverseMap();
        CreateMap<Review, ReviewDto>().ReverseMap();
    }
}
