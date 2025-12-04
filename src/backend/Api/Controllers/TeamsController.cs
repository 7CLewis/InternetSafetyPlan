using InternetSafetyPlan.Application.TeamAggregate.Commands;
using InternetSafetyPlan.Application.TeamAggregate.Queries;
using InternetSafetyPlan.Domain.Base;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace InternetSafetyPlan.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
//[Authorize]
public class TeamsController : ApiController
{
    public TeamsController(ISender sender)
        : base(sender) { }

    /// <summary>
    /// Gets a team.
    /// </summary>
    /// <param name="id">The unique identifier of the team.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The team, if it exists.</returns>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(Result<List<TeamByIdResponse>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetTeamById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var query = new GetTeamByIdQuery(id);

        var result = await Sender.Send(query, cancellationToken);

        if (result.IsFailure) return HandleFailure(result);

        return Ok(result);
    }

    /// <summary>
    /// Gets a teammate.
    /// </summary>
    /// <param name="id">The unique identifier of the teammate.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The team, if it exists.</returns>
    [HttpGet("{teamId:guid}/teammates/{teammateId:guid}")]
    [ProducesResponseType(typeof(Result<List<TeammateByIdResponse>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetTeammateById([FromRoute] Guid teamId, [FromRoute] Guid teammateId, CancellationToken cancellationToken)
    {
        var query = new GetTeammateByIdQuery(teamId, teammateId);

        var result = await Sender.Send(query, cancellationToken);

        if (result.IsFailure) return HandleFailure(result);

        return Ok(result);
    }

    /// <summary>
    /// Creates a Team
    /// </summary>
    /// <param name="command">The command</param>
    /// <param name="cancellationToken"></param>
    /// <returns>True if creation was successful; false otherwise</returns>
    [HttpPost]
    [ProducesResponseType(typeof(Result<Guid>), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateTeam([FromBody] CreateTeamCommand command, CancellationToken cancellationToken)
    {
        var result = await Sender.Send(command, cancellationToken);

        if (result.IsFailure) return HandleFailure(result);

        return CreatedAtAction(nameof(GetTeamById), new { id = result.Value }, result.Value);
    }

    /// <summary>
    /// Updates team information.
    /// </summary>
    /// <param name="command">The command</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPut]
    [ProducesResponseType(typeof(Result<Unit>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateTeamInformation([FromBody] UpdateTeamInformationCommand command, CancellationToken cancellationToken)
    {
        var result = await Sender.Send(command, cancellationToken);

        if (result.IsFailure) return HandleFailure(result);

        return Ok(result);
    }

    /// <summary>
    /// Edits a teammate.
    /// </summary>
    /// <param name="command">The command</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPut("teammates")]
    [ProducesResponseType(typeof(Result<Unit>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateTeammateInformation([FromBody] UpdateTeammateInformationCommand command, CancellationToken cancellationToken)
    {
        var result = await Sender.Send(command, cancellationToken);

        if (result.IsFailure) return HandleFailure(result);

        return Ok(result);
    }

    /// <summary>
    /// Deletes a Team
    /// </summary>
    /// <param name="id">The ID of the Team to be deleted</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(Result<Unit>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteTeam([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var command = new DeleteTeamCommand(id);

        var result = await Sender.Send(command, cancellationToken);

        if (result.IsFailure) return HandleFailure(result);

        return Ok(result);
    }

    /// <summary>
    /// Adds a teammate to a team. 
    /// This is a creation of a Teammate, as it is
    /// a child entity under Team, which is the aggregate root.
    /// </summary>
    /// <param name="command">The command</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost("teammates")]
    [ProducesResponseType(typeof(Result<Guid>), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddTeammateToTeam([FromBody] AddTeammateToTeamCommand command, CancellationToken cancellationToken)
    {
        var result = await Sender.Send(command, cancellationToken);

        if (result.IsFailure) return HandleFailure(result);

        return CreatedAtAction(nameof(AddTeammateToTeam), new { id = result.Value });
    }

    /// <summary>
    /// Deletes a teammate from a team
    /// </summary>
    /// <param name="command">The command</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpDelete("teammates")]
    [ProducesResponseType(typeof(Result<Unit>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteTeammate([FromBody] DeleteTeammateCommand command, CancellationToken cancellationToken)
    {
        var result = await Sender.Send(command, cancellationToken);

        if (result.IsFailure) return HandleFailure(result);

        return Ok(result);
    }
}
