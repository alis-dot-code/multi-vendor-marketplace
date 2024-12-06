using MarketNest.Application.Common.Exceptions;
using FluentValidation;

namespace MarketNest.API.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
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

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        int statusCode;
        object error;

        if (exception is ValidationException ex)
        {
            statusCode = 400;
            error = new
            {
                status = 400,
                title = "Validation Error",
                detail = "One or more validation errors occurred",
                errors = ex.Errors.GroupBy(e => e.PropertyName)
                    .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray())
            };
        }
        else if (exception is BadRequestException badEx)
        {
            statusCode = 400;
            error = new { status = 400, title = "Bad Request", detail = badEx.Message };
        }
        else if (exception is NotFoundException notFoundEx)
        {
            statusCode = 404;
            error = new { status = 404, title = "Not Found", detail = notFoundEx.Message };
        }
        else if (exception is ForbiddenException forbiddenEx)
        {
            statusCode = 403;
            error = new { status = 403, title = "Forbidden", detail = forbiddenEx.Message };
        }
        else if (exception is ConflictException conflictEx)
        {
            statusCode = 409;
            error = new { status = 409, title = "Conflict", detail = conflictEx.Message };
        }
        else
        {
            statusCode = 500;
            error = new { status = 500, title = "Internal Server Error", detail = "An unexpected error occurred" };
        }

        if (statusCode == 500)
        {
            _logger.LogError(exception, "Unhandled exception");
        }

        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";
        await System.Text.Json.JsonSerializer.SerializeAsync(context.Response.Body, error);
    }
}
