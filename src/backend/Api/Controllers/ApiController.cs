using InternetSafetyPlan.Domain.Base;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace InternetSafetyPlan.Api.Controllers;

/// <summary>
/// Represents the base API controller.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ApiController : ControllerBase
{
    private readonly ISender _sender;

    /// <summary>
    /// Constructs an ApiController instance, setting _sender
    /// to the injected MediatR sender dependency
    /// </summary>
    /// <param name="sender">The MediatR request sender service</param>
    public ApiController(ISender sender) =>
        _sender = sender;

    /// <summary>
    /// Gets the sender.
    /// </summary>
    protected ISender Sender => _sender;

    /// <summary>
    /// Hamdles failures
    /// </summary>
    /// <param name="result"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    protected IActionResult HandleFailure(Result result) =>
    result switch
    {
        { IsSuccess: true } => throw new InvalidOperationException(),
        { Error.Code: "Database Error" } =>
            StatusCode(500,
                CreateProblemDetails(
                    "Database Error", StatusCodes.Status500InternalServerError,
                    result.Error)),
        IValidationResult validationResult =>
            BadRequest(
                CreateProblemDetails(
                    "Validation Error", StatusCodes.Status400BadRequest,
                    result.Error,
                    validationResult.Errors)),
        _ =>
            BadRequest(
                CreateProblemDetails(
                    "Bad Request",
                    StatusCodes.Status400BadRequest,
                    result.Error))
    };

    /// <summary>
    /// Create a structured list of problem details based on the error(s)
    /// </summary>
    /// <param name="title"></param>
    /// <param name="status"></param>
    /// <param name="error"></param>
    /// <param name="errors"></param>
    /// <returns></returns>
    private static ProblemDetails CreateProblemDetails(
        string title,
        int status,
        Error error,
        Error[]? errors = null) =>
        new()
        {
            Title = title,
            Type = error.Code,
            Detail = error.Message,
            Status = status,
            Extensions = { { nameof(errors), errors } }
        };
}
