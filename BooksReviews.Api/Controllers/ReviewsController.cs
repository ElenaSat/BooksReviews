using BooksReviews.Application.Features.Reviews.Commands.CreateReview;
using BooksReviews.Application.Features.Reviews.Commands.DeleteReview;
using BooksReviews.Application.Features.Reviews.DTOs;
using BooksReviews.Application.Features.Reviews.Queries.GetReviewsByBookId;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace BooksReviews.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReviewsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ReviewsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("book/{bookId}")]
    public async Task<ActionResult<List<ReviewDto>>> GetByBookId(string bookId)
    {
        var query = new GetReviewsByBookIdQuery(bookId);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<string>> Create(CreateReviewCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<ActionResult> Delete(string id)
    {
        var command = new DeleteReviewCommand(id);
        await _mediator.Send(command);
        return NoContent();
    }
}
