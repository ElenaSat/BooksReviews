# syntax=docker/dockerfile:1

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0-alpine AS build
WORKDIR /src

# Copy project files for dependency resolution
COPY BooksReviews.Domain/BooksReviews.Domain.csproj BooksReviews.Domain/
COPY BooksReviews.Application/BooksReviews.Application.csproj BooksReviews.Application/
COPY BooksReviews.Infrastructure/BooksReviews.Infrastructure.csproj BooksReviews.Infrastructure/
COPY BooksReviews.Api/BooksReviews.Api.csproj BooksReviews.Api/

# Restore dependencies
RUN dotnet restore BooksReviews.Api/BooksReviews.Api.csproj

# Copy remaining source code
COPY BooksReviews.Domain/ BooksReviews.Domain/
COPY BooksReviews.Application/ BooksReviews.Application/
COPY BooksReviews.Infrastructure/ BooksReviews.Infrastructure/
COPY BooksReviews.Api/ BooksReviews.Api/

# Build and publish
WORKDIR /src/BooksReviews.Api
RUN dotnet publish \
    --configuration Release \
    --no-restore \
    --output /app/publish \
    /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0-alpine AS runtime
WORKDIR /app

# 🔧 GLOBALIZATION + EF SQL FIX
RUN apk add --no-cache icu-libs icu-data-full libintl
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=0

# Create non-root user
RUN addgroup -g 1001 -S appgroup && \
    adduser -u 1001 -S appuser -G appgroup

# Copy published application
COPY --from=build /app/publish .

# Set ownership
RUN chown -R appuser:appgroup /app

# Switch to non-root user
USER appuser

# Expose port
EXPOSE 5216

# Health check
HEALTHCHECK --interval=30s --timeout=3s --start-period=5s --retries=3 \
    CMD wget --no-verbose --tries=1 --spider http://localhost:5216/health || exit 1

ENTRYPOINT ["dotnet", "BooksReviews.Api.dll"]
