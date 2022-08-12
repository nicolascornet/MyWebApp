using MyWebApp.Exceptions;
using System.Net;
using System.Text.Json;

namespace MyWebApp.Common;

public class CustomExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<CustomExceptionHandlerMiddleware> _logger;

    public CustomExceptionHandlerMiddleware(RequestDelegate next, ILogger<CustomExceptionHandlerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        _logger.LogError(exception, "Catched exception");
        context.Response.ContentType = "application/json";

        while (exception is AggregateException)
            exception = exception.InnerException;

        context.Response.StatusCode = exception switch
        {
            ValidationException _ => (int)HttpStatusCode.BadRequest,
            NotFoundException _ => (int)HttpStatusCode.NotFound,
            ForbiddenException _ => (int)HttpStatusCode.Forbidden,
            _ => (int)HttpStatusCode.InternalServerError
        };

        return context.Response.WriteAsync(JsonSerializer.Serialize(new { Error = exception.Message }));
    }
}
