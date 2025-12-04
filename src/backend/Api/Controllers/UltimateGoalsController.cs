using InternetSafetyPlan.Application.UltimateGoalAggregate.Commands;
using InternetSafetyPlan.Application.UltimateGoalAggregate.Queries;
using InternetSafetyPlan.Domain.Base;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace InternetSafetyPlan.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
//[Authorize]
public class UltimateGoalsController : ApiController
{
    public UltimateGoalsController(ISender sender)
        : base(sender) { }

    /// <summary>
    /// Gets the ultimate goal with the specified ID
    /// </summary>
    /// <param name="id ">The team identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns></returns>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(Result<UltimateGoalByIdResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetUltimateGoalById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var query = new GetUltimateGoalByIdQuery(id);

        var result = await Sender.Send(query, cancellationToken);

        if (result.IsFailure) return HandleFailure(result);

        return Ok(result);
    }

    /// <summary>
    /// Gets a team's ultimate goals.
    /// </summary>
    /// <param name="teamId ">The unique identifier of the team.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The team, if it exists.</returns>
    [HttpGet("teams/{teamId:guid}")]
    [ProducesResponseType(typeof(Result<List<TeamUltimateGoalsResponse>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetTeamUltimateGoals(
        [FromRoute] Guid teamId,
        CancellationToken cancellationToken,
        [FromQuery] bool includeGoalsAndActions = false)
    {
        var query = new GetTeamUltimateGoalsQuery(teamId, includeGoalsAndActions);

        var result = await Sender.Send(query, cancellationToken);

        if (result.IsFailure) return HandleFailure(result);

        return Ok(result);
    }

    /// <summary>
    /// Creates a new ultimate goal under a team.
    /// </summary>
    /// <param name="command">The command</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType(typeof(Result<Guid>), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateUltimateGoal([FromBody] CreateUltimateGoalCommand command, CancellationToken cancellationToken)
    {
        var result = await Sender.Send(command, cancellationToken);

        if (result.IsFailure) return HandleFailure(result);

        return CreatedAtAction(nameof(GetUltimateGoalById), new { id = result.Value }, result.Value);
    }

    /// <summary>
    /// Updates ultimate goal information.
    /// </summary>
    /// <param name="command">The command</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPut]
    [ProducesResponseType(typeof(Result<Unit>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateUltimateGoalInformation([FromBody] UpdateUltimateGoalInformationCommand command, CancellationToken cancellationToken)
    {
        var result = await Sender.Send(command, cancellationToken);

        if (result.IsFailure) return HandleFailure(result);

        return Ok(result);
    }

    /// <summary>
    /// Deletes an ultimate goal
    /// </summary>
    /// <param name="id">The ID of the Ultimate Goal to be deleted</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(Result<Unit>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteUltimateGoal([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var command = new DeleteUltimateGoalCommand(id);

        var result = await Sender.Send(command, cancellationToken);

        if (result.IsFailure) return HandleFailure(result);

        return Ok(result);
    }
}
