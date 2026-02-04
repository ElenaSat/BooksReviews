
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy project files (paths relative to repository root)
COPY BooksReviews.Api/BooksReviews.Api.csproj BooksReviews.Api/
COPY BooksReviews.Application/BooksReviews.Application.csproj BooksReviews.Application/
COPY BooksReviews.Infrastructure/BooksReviews.Infrastructure.csproj BooksReviews.Infrastructure/
COPY BooksReviews.Domain/BooksReviews.Domain.csproj BooksReviews.Domain/

RUN dotnet restore BooksReviews.Api/BooksReviews.Api.csproj

# Copy the rest of the repository
COPY . .
WORKDIR /src/BooksReviews.Api
RUN dotnet publish "BooksReviews.Api.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:80
COPY --from=build /app/publish ./
EXPOSE 80
ENTRYPOINT ["dotnet", "BooksReviews.Api.dll"]
