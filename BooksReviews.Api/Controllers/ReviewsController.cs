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
public class ReviewsController : ApiControllerBase
{
    [HttpGet("book/{bookId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<ReviewDto>>> GetByBookId(string bookId)
    {
        var result = await Mediator.Send(new GetReviewsByBookIdQuery(bookId));
        return HandleResult(result);
    }

    [HttpPost]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<string>> Create(CreateReviewCommand command)
    {
        var result = await Mediator.Send(command);
        if (result.IsSuccess)
            return CreatedAtAction(nameof(GetByBookId), new { bookId = command.BookId }, result.Value);
        
        return BadRequest(new { message = result.Error });
    }

    [HttpDelete("{id}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Delete(string id)
    {
        var result = await Mediator.Send(new DeleteReviewCommand(id));
        return HandleResult(result);
    }
}
