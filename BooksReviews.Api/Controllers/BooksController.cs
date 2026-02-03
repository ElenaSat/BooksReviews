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
public class BooksController : ControllerBase
{
    private readonly IMediator _mediator;

    public BooksController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<List<BookDto>>> GetAll()
    {
        var query = new GetBooksQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<BookDto>> GetById(string id)
    {
        var query = new GetBookByIdQuery(id);
        var result = await _mediator.Send(query);
        if (result == null) return NotFound();
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<string>> Create(CreateBookCommand command)
    {
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = result }, result);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(string id, UpdateBookCommand command)
    {
        if (id != command.Id) return BadRequest();
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(string id)
    {
        var command = new DeleteBookCommand(id);
        await _mediator.Send(command);
        return NoContent();
    }
}
