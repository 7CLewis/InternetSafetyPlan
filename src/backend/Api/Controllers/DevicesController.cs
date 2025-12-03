using InternetSafetyPlan.Application.DeviceAggregate.Commands;
using InternetSafetyPlan.Application.DeviceAggregate.Queries;
using InternetSafetyPlan.Domain.Base;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace InternetSafetyPlan.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
//[Authorize]
public class DevicesController : ApiController
{
    public DevicesController(ISender sender)
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
    public async Task<IActionResult> GetDeviceById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var query = new GetDeviceByIdQuery(id);

        var result = await Sender.Send(query, cancellationToken);

        if (result.IsFailure) return HandleFailure(result);

        return Ok(result);
    }

    /// <summary>
    /// Gets the devices (if there are any) associated with a team.
    /// </summary>
    /// <param name="teamId ">The team identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The team's devices, if any.</returns>
    [HttpGet("teams/{teamId:guid}")]
    [ProducesResponseType(typeof(Result<List<TeamDevicesResponse>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetTeamDevices([FromRoute] Guid teamId, CancellationToken cancellationToken)
    {
        var query = new GetTeamDevicesQuery(teamId);

        var result = await Sender.Send(query, cancellationToken);

        if (result.IsFailure) return HandleFailure(result);

        return Ok(result);
    }

    /// <summary>
    /// Creates a new device under a given team.
    /// </summary>
    /// <param name="command">The command</param>
    /// <param name="cancellationToken"></param>
    /// <returns>The ID of the newly-created device if successful</returns>
    [HttpPost]
    [ProducesResponseType(typeof(Result<Guid>), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddDevice([FromBody] AddDeviceCommand command, CancellationToken cancellationToken)
    {
        var result = await Sender.Send(command, cancellationToken);

        if (result.IsFailure) return HandleFailure(result);

        return CreatedAtAction(nameof(GetDeviceById), new { id = result.Value }, result.Value);
    }

    /// <summary>
    /// Updates information about a device.
    /// </summary>
    /// <param name="command">The command</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPut]
    [ProducesResponseType(typeof(Result<Unit>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateDeviceInformation([FromBody] UpdateDeviceInformationCommand command, CancellationToken cancellationToken)
    {
        var result = await Sender.Send(command, cancellationToken);

        if (result.IsFailure) return HandleFailure(result);

        return Ok(result);
    }

    /// <summary>
    /// Deletes a device
    /// </summary>
    /// <param name="id">The ID of the Device to be deleted.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(Result<Unit>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteDevice([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var command = new DeleteDeviceCommand(id);

        var result = await Sender.Send(command, cancellationToken);

        if (result.IsFailure) return HandleFailure(result);

        return Ok(result);
    }

    /// <summary>
    /// Adds a tag to a device.
    /// </summary>
    /// <param name="command">The command</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPut("tags")]
    [ProducesResponseType(typeof(Result<Unit>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddTagToDevice([FromBody] AddTagToDeviceCommand command, CancellationToken cancellationToken)
    {
        var result = await Sender.Send(command, cancellationToken);

        if (result.IsFailure) return HandleFailure(result);

        return Ok(result);
    }

    /// <summary>
    /// Removes a tag from a device
    /// </summary>
    /// <param name="command">The command</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpDelete("tags")]
    [ProducesResponseType(typeof(Result<Unit>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RemoveTagFromDevice([FromBody] RemoveTagFromDeviceCommand command, CancellationToken cancellationToken)
    {
        var result = await Sender.Send(command, cancellationToken);

        if (result.IsFailure) return HandleFailure(result);

        return Ok(result);
    }

    /// <summary>
    /// Adds a teammate to a device, representing that this teammate
    /// is a user or otherwise has access to the device.
    /// </summary>
    /// <param name="command">The command</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPut("teammates")]
    [ProducesResponseType(typeof(Result<Unit>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddTeammateToDevice([FromBody] AddTeammateToDeviceCommand command, CancellationToken cancellationToken)
    {
        var result = await Sender.Send(command, cancellationToken);

        if (result.IsFailure) return HandleFailure(result);

        return Ok(result);
    }

    /// <summary>
    /// Removes a teammate from a device.
    /// </summary>
    /// <param name="command">The command</param
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpDelete("teammates")]
    [ProducesResponseType(typeof(Result<Unit>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RemoveTeammateFromDevice([FromBody] RemoveTeammateFromDeviceCommand command, CancellationToken cancellationToken)
    {
        var result = await Sender.Send(command, cancellationToken);

        if (result.IsFailure) return HandleFailure(result);

        return Ok(result);
    }
}
