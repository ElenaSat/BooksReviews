using BooksReviews.Application.Common.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BooksReviews.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class ApiControllerBase : ControllerBase
{
    private IMediator? _mediator;
    protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<IMediator>();

    protected ActionResult HandleResult<T>(Result<T> result)
    {
        if (result == null) return NotFound();

        if (result.IsSuccess)
        {
            if (result.Value == null) return NoContent();
            return Ok(result.Value);
        }

        return result.Error switch
        {
            "Not Found" => NotFound(),
            _ => BadRequest(new { message = result.Error })
        };
    }
}
