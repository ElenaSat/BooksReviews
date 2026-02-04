using BooksReviews.Application.Features.Users.Commands.CreateUser;
using BooksReviews.Application.Features.Users.Commands.DeleteUser;
using BooksReviews.Application.Features.Users.Commands.UpdateUser;
using BooksReviews.Application.Features.Users.Commands.Login;
using BooksReviews.Application.Features.Users.DTOs;
using BooksReviews.Application.Features.Users.Queries.GetUserById;
using BooksReviews.Application.Features.Users.Queries.GetUsers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BooksReviews.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ApiControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<UserDto>>> GetAll()
    {
        var result = await Mediator.Send(new GetUsersQuery());
        return HandleResult(result);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserDto>> GetById(string id)
    {
        var result = await Mediator.Send(new GetUserByIdQuery(id));
        return HandleResult(result);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<string>> Create(CreateUserCommand command)
    {
        var result = await Mediator.Send(command);
        if (result.IsSuccess)
            return CreatedAtAction(nameof(GetById), new { id = result.Value }, result.Value);
        
        return BadRequest(new { message = result.Error });
    }

    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<LoginResponse>> Login(LoginCommand command)
    {
        var result = await Mediator.Send(command);
        if (result.IsSuccess) return Ok(result.Value);
        return Unauthorized(new { message = result.Error });
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Update(string id, UpdateUserCommand command)
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
        var result = await Mediator.Send(new DeleteUserCommand(id));
        return HandleResult(result);
    }
}
