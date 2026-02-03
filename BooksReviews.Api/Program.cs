using BooksReviews.Application;
using BooksReviews.Infrastructure;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.PropertyNameCaseInsensitive = true;
    options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
});
// Add services to the container.
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/openapi/v1.json", "BooksReviews v1"); // usa el OpenAPI de .NET 9
    });
}

app.UseCors(options => {
    options.AllowAnyHeader();
    options.AllowAnyMethod();
    options.AllowAnyOrigin();

});
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
