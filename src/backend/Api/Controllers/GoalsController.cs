using InternetSafetyPlan.Application.DeviceAggregate.Queries;
using InternetSafetyPlan.Application.GoalAggregate.Commands;
using InternetSafetyPlan.Application.GoalAggregate.Queries;
using InternetSafetyPlan.Application.TeamAggregate.Commands;
using InternetSafetyPlan.Domain.Base;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace InternetSafetyPlan.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
//[Authorize]
public class GoalsController : ApiController
{
    public GoalsController(ISender sender)
        : base(sender) { }

    /// <summary>
    /// Gets the device with the specified ID
    /// </summary>
    /// <param name="id ">The team identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns></returns>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(Result<DeviceByIdResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetGoalById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var query = new GetGoalByIdQuery(id);

        var result = await Sender.Send(query, cancellationToken);

        if (result.IsFailure) return HandleFailure(result);

        return Ok(result);
    }

    /// <summary>
    /// Gets the goals (if there are any) associated with a team.
    /// </summary>
    /// <param name="teamId ">The team identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The team's devices, if any.</returns>
    [HttpGet("teams/{teamId:guid}")]
    [ProducesResponseType(typeof(Result<List<TeamGoalsResponse>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetTeamGoals([FromRoute] Guid teamId, CancellationToken cancellationToken)
    {
        var query = new GetTeamGoalsQuery(teamId);

        var result = await Sender.Send(query, cancellationToken);

        if (result.IsFailure) return HandleFailure(result);

        return Ok(result);
    }

    /// <summary>
    /// Gets the goals (if there are any) associated with a team.
    /// </summary>
    /// <param name="teamId ">The team identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The team's devices, if any.</returns>
    [HttpGet("suggested")]
    [ProducesResponseType(typeof(Result<List<SuggestedGoalsResponse>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetSuggestedGoals(CancellationToken cancellationToken)
    {
        var query = new GetSuggestedGoalsQuery();

        var result = await Sender.Send(query, cancellationToken);

        if (result.IsFailure) return HandleFailure(result);

        return Ok(result);
    }

    /// <summary>
    /// Gets the action items (if there are any) associated with a team.
    /// </summary>
    /// <param name="teamId ">The team identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The team's devices, if any.</returns>
    [HttpGet("teams/{teamId:guid}/action-items")]
    [ProducesResponseType(typeof(Result<List<TeamActionItemsResponse>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetTeamActionItems([FromRoute] Guid teamId, CancellationToken cancellationToken)
    {
        var query = new GetTeamActionItemsQuery(teamId);

        var result = await Sender.Send(query, cancellationToken);

        if (result.IsFailure) return HandleFailure(result);

        return Ok(result);
    }

    /// <summary>
    /// Gets the action items associated with a team and ID.
    /// </summary>
    /// <param name="teamId ">The team identifier.</param>
    /// <param name="goalId ">The goal identifier.</param>
    /// <param name="actionItemId ">The action item identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The team's devices, if any.</returns>
    [HttpGet("teams/{teamId:guid}/goals/{goalId:guid}/action-items/{actionItemId:guid}")]
    [ProducesResponseType(typeof(Result<List<TeamActionItemsResponse>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetActionItemById([FromRoute] Guid teamId, [FromRoute] Guid goalId, [FromRoute] Guid actionItemId, CancellationToken cancellationToken)
    {
        var query = new GetActionItemByIdQuery(teamId, goalId, actionItemId);

        var result = await Sender.Send(query, cancellationToken);

        if (result.IsFailure) return HandleFailure(result);

        return Ok(result);
    }

    /// <summary>
    /// Gets the action items (if there are any) associated with a goal.
    /// </summary>
    /// <param name="teamId ">The team identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The team's devices, if any.</returns>
    [HttpGet("action-items/{actionItem:guid}")]
    [ProducesResponseType(typeof(Result<List<TeamGoalsResponse>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetActionItemsByGoalId([FromRoute] Guid actionItem, CancellationToken cancellationToken)
    {
        var query = new GetTeamGoalsQuery(actionItem);

        var result = await Sender.Send(query, cancellationToken);

        if (result.IsFailure) return HandleFailure(result);

        return Ok(result);
    }

    /// <summary>
    /// Creates a Goal
    /// </summary>
    /// <param name="command">The command</param>
    /// <param name="cancellationToken"></param>
    /// <returns>True if creation was successful; false otherwise</returns>
    [HttpPost]
    [ProducesResponseType(typeof(Result<Guid>), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateGoalAndActionItems([FromBody] CreateGoalAndActionItemsCommand command, CancellationToken cancellationToken)
    {
        var result = await Sender.Send(command, cancellationToken);

        if (result.IsFailure) return HandleFailure(result);

        return CreatedAtAction(nameof(GetGoalById), new { id = result.Value }, result.Value);
    }

    /// <summary>
    /// Edits a Goal and its Action Items
    /// </summary>
    /// <param name="command">The command</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Unit</returns>
    [HttpPut]
    [ProducesResponseType(typeof(Result<Unit>), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> EditGoalAndActionItems([FromBody] EditGoalAndActionItemsCommand command, CancellationToken cancellationToken)
    {
        var result = await Sender.Send(command, cancellationToken);

        if (result.IsFailure) return HandleFailure(result);

        return Ok(result);
    }

    /// <summary>
    /// Edits an Action Item.
    /// </summary>
    /// <param name="command">The command</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPut("action-items/{id:guid}")]
    [ProducesResponseType(typeof(Result<Unit>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> EditActionItem([FromBody] EditActionItemCommand command, CancellationToken cancellationToken)
    {
        var result = await Sender.Send(command, cancellationToken);

        if (result.IsFailure) return HandleFailure(result);

        return Ok(result);
    }

    /// <summary>
    /// Toggles an Action Item's completion status
    /// </summary>
    /// <param name="command">The command</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPut("action-items/{id:guid}/toggleCompletion")]
    [ProducesResponseType(typeof(Result<Unit>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ToggleActionItemCompletion([FromBody] ToggleActionItemCompletionCommand command, CancellationToken cancellationToken)
    {
        var result = await Sender.Send(command, cancellationToken);

        if (result.IsFailure) return HandleFailure(result);

        return Ok(result);
    }

    /// <summary>
    /// Deletes a goal
    /// </summary>
    /// <param name="id">The ID of the Goal to be deleted</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(Result<Unit>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteGoal([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var command = new DeleteGoalCommand(id);

        var result = await Sender.Send(command, cancellationToken);

        if (result.IsFailure) return HandleFailure(result);

        return Ok(result);
    }

    /// <summary>
    /// Adds an Action Item to a Goal.
    /// This is a creation of an Action Item, as it is
    /// a child entity under Goal, which is the aggregate root.
    /// </summary>
    /// <param name="command">The command</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost("action-items")]
    [ProducesResponseType(typeof(Result<Guid>), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddActionItemToGoal([FromBody] AddActionItemToGoalCommand command, CancellationToken cancellationToken)
    {
        var result = await Sender.Send(command, cancellationToken);

        if (result.IsFailure) return HandleFailure(result);

        return CreatedAtAction(nameof(GetTeamGoals), new { id = result.Value }); // TODO: nameof(GetActionItemById)
    }

    /// <summary>
    /// Deletes an Action Item from a Goal.
    /// </summary>
    /// <param name="command">The command</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpDelete("action-items")]
    [ProducesResponseType(typeof(Result<Unit>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteActionItem([FromBody] DeleteActionItemCommand command, CancellationToken cancellationToken)
    {
        var result = await Sender.Send(command, cancellationToken);

        if (result.IsFailure) return HandleFailure(result);

        return Ok(result);
    }

    /// <summary>
    /// Adds a device to the list of devices that are
    /// affected by the goal
    /// </summary>
    /// <param name="command">The command</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPut("devices")]
    [ProducesResponseType(typeof(Result<Unit>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddAffectedDeviceToGoal([FromBody] AddAffectedDeviceToGoalCommand command, CancellationToken cancellationToken)
    {
        var result = await Sender.Send(command, cancellationToken);

        if (result.IsFailure) return HandleFailure(result);

        return Ok(result);
    }

    /// <summary>
    /// Removes a device from the list of devices
    /// affected by the goal.
    /// </summary>
    /// <param name="command">The command</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpDelete("devices")]
    [ProducesResponseType(typeof(Result<Unit>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RemoveAffectedDeviceFromGoal([FromBody] RemoveAffectedDeviceFromGoalCommand command, CancellationToken cancellationToken)
    {
        var result = await Sender.Send(command, cancellationToken);

        if (result.IsFailure) return HandleFailure(result);

        return Ok(result);
    }

    /// <summary>
    /// Adds a tag to a goal.
    /// </summary>
    /// <param name="command">The command</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPut("tags")]
    [ProducesResponseType(typeof(Result<Unit>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddTagToGoal([FromBody] AddTagToGoalCommand command, CancellationToken cancellationToken)
    {
        var result = await Sender.Send(command, cancellationToken);

        if (result.IsFailure) return HandleFailure(result);

        return Ok(result);
    }

    /// <summary>
    /// Removes a tag from a goal
    /// </summary>
    /// <param name="command">The command</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpDelete("tags")]
    [ProducesResponseType(typeof(Result<Unit>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RemoveTagFromGoal([FromBody] RemoveTagFromGoalCommand command, CancellationToken cancellationToken)
    {
        var result = await Sender.Send(command, cancellationToken);

        if (result.IsFailure) return HandleFailure(result);

        return Ok(result);
    }
}
