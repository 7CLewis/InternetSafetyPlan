using InternetSafetyPlan.Application.UserAggregate.Commands;
using InternetSafetyPlan.Application.UserAggregate.Queries;
using InternetSafetyPlan.Domain.Base;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace InternetSafetyPlan.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
//[Authorize]
public class UsersController : ApiController
{
    public UsersController(ISender sender)
        : base(sender) { }

    /// <summary>
    /// Gets the user with the specified ID
    /// </summary>
    /// <param name="id ">The user identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns></returns>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(Result<UserByIdResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetUserById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var query = new GetUserByIdQuery(id);

        var result = await Sender.Send(query, cancellationToken);

        if (result.IsFailure) return HandleFailure(result);

        return Ok(result);
    }

    /// <summary>
    /// Gets the user with the specified email
    /// </summary>
    /// <param name="email ">The email address.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns></returns>
    [HttpGet("{email}")]
    [ProducesResponseType(typeof(Result<UserByEmailResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetUserByEmail([FromRoute] string email, CancellationToken cancellationToken)
    {
        var query = new GetUserByEmailQuery(email);

        var result = await Sender.Send(query, cancellationToken);

        if (result.IsFailure) return HandleFailure(result);

        return Ok(result);
    }

    /// <summary>
    /// Gets the team that the user is a member of
    /// </summary>
    /// <param name="email ">The email address.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns></returns>
    [HttpGet("{email}/team")]
    [ProducesResponseType(typeof(Result<TeamByUserEmailResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetTeamByUserEmail([FromRoute] string email, CancellationToken cancellationToken)
    {
        var query = new GetTeamByUserEmailQuery(email);

        var result = await Sender.Send(query, cancellationToken);

        if (result.IsFailure) return HandleFailure(result);

        return Ok(result);
    }

    /// <summary>
    /// Creates a user
    /// </summary>
    /// <param name="command">The command</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType(typeof(Result<Guid>), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserCommand command, CancellationToken cancellationToken)
    {
        var result = await Sender.Send(command, cancellationToken);

        if (result.IsFailure) return HandleFailure(result);

        return CreatedAtAction(nameof(GetUserById), new { id = result.Value }, result.Value);
    }

    /// <summary>
    /// Modifies user's email.
    /// </summary>
    /// <param name="command">The command</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPut]
    [ProducesResponseType(typeof(Result<Unit>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ModifyUserEmail([FromBody] ModifyUserEmailCommand command, CancellationToken cancellationToken)
    {
        var result = await Sender.Send(command, cancellationToken);

        if (result.IsFailure) return HandleFailure(result);

        return Ok(result);
    }

    /// <summary>
    /// Deletes a user
    /// </summary>
    /// <param name="id">The ID of the user to be deleted</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(Result<Unit>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteUser([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var command = new DeleteUserCommand(id);

        var result = await Sender.Send(command, cancellationToken);

        if (result.IsFailure) return HandleFailure(result);

        return Ok(result);
    }
}
