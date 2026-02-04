using BooksReviews.Application.Features.Books.Commands.CreateBook;
using BooksReviews.Application.Features.Books.Commands.DeleteBook;
using BooksReviews.Application.Features.Books.Commands.UpdateBook;
using BooksReviews.Application.Features.Books.DTOs;
using BooksReviews.Application.Features.Books.Queries.GetBookById;
using BooksReviews.Application.Features.Books.Queries.GetBooks;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BooksReviews.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ApiControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<BookDto>>> GetAll()
    {
        var result = await Mediator.Send(new GetBooksQuery());
        return HandleResult(result);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<BookDto>> GetById(string id)
    {
        var result = await Mediator.Send(new GetBookByIdQuery(id));
        return HandleResult(result);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<string>> Create(CreateBookCommand command)
    {
        var result = await Mediator.Send(command);
        if (result.IsSuccess)
            return CreatedAtAction(nameof(GetById), new { id = result.Value }, result.Value);
        
        return BadRequest(new { message = result.Error });
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Update(string id, UpdateBookCommand command)
    {
        if (id != command.Id) return BadRequest(new { message = "ID mismatch" });
        var result = await Mediator.Send(command);
        return HandleResult(result);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Delete(string id)
    {
        var command = new DeleteBookCommand(id);
        await Mediator.Send(command);
        return NoContent();
    }
}
