using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace InternetSafetyPlan.Api.Middleware;

internal sealed class ExceptionHandlingMiddleware : IMiddleware
{
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger) => _logger = logger;

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);

            await HandleExceptionAsync(context, e);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
    {

        httpContext.Response.ContentType = "application/json";

        httpContext.Response.StatusCode = exception switch
        {
            Exception or ValidationException => StatusCodes.Status400BadRequest,
            _ => StatusCodes.Status500InternalServerError
        };

        var errors = Array.Empty<ApiError>();

        var response = new
        {
            status = httpContext.Response.StatusCode,
            message = exception.Message,
            errors
        };

        await httpContext.Response.WriteAsync(JsonSerializer.Serialize(response));
    }

    private record ApiError(string PropertyName, string ErrorMessage);
}
