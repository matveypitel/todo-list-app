using System.Net;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using TodoListApp.Models.DTOs;

namespace TodoListApp.WebApi.Middleware;

/// <summary>
/// Middleware for handling exceptions and producing error responses.
/// </summary>
public class ExceptionHandlingMiddleware
{
    private static readonly JsonSerializerOptions JsonSerializerOptions = new JsonSerializerOptions()
    {
        WriteIndented = true,
    };

    private static readonly Action<ILogger, int, string, Exception> LogError = LoggerMessage.Define<int, string>(
        LogLevel.Error,
        default,
        "Produce error response: Error: [{Code}] | Message: [{ExMessage}]");

    private readonly RequestDelegate next;
    private readonly ILogger<ExceptionHandlingMiddleware> logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="ExceptionHandlingMiddleware"/> class.
    /// </summary>
    /// <param name="next">The next middleware in the pipeline.</param>
    /// <param name="logger">The logger.</param>
    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        this.next = next;
        this.logger = logger;
    }

    /// <summary>
    /// Invokes the middleware.
    /// </summary>
    /// <param name="httpContext">The HTTP context.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task InvokeAsync(HttpContext httpContext)
    {
        ArgumentNullException.ThrowIfNull(httpContext);

        try
        {
            await this.next(httpContext);
        }
        catch (KeyNotFoundException ex)
        {
            await this.HandleExceptionAsync(
                httpContext,
                HttpStatusCode.NotFound,
                ex,
                "Not Found.");
        }
        catch (ArgumentNullException ex)
        {
            await this.HandleExceptionAsync(
                httpContext,
                HttpStatusCode.BadRequest,
                ex,
                "Invalid Request.");
        }
        catch (UnauthorizedAccessException ex)
        {
            await this.HandleExceptionAsync(
                httpContext,
                HttpStatusCode.Forbidden,
                ex,
                "Forbidden.");
        }
        catch (DbUpdateException ex)
        {
            await this.HandleExceptionAsync(
                httpContext,
                HttpStatusCode.Conflict,
                ex,
                "Conflict operation.");
        }
        catch (Exception ex) when
            (ex is not KeyNotFoundException
            && ex is not ArgumentNullException
            && ex is not UnauthorizedAccessException)
        {
            await this.HandleExceptionAsync(
                httpContext,
                HttpStatusCode.InternalServerError,
                ex,
                "Internal Server Error.");
        }
    }

    private async Task HandleExceptionAsync(
        HttpContext context,
        HttpStatusCode statusCode,
        Exception ex,
        string message)
    {
        LogError(this.logger, (int)statusCode, ex.Message, ex);

        HttpResponse response = context.Response;

        response.ContentType = "application/json";
        response.StatusCode = (int)statusCode;

        ErrorDto error = new ErrorDto
        {
            StatusCode = (int)statusCode,
            Message = message,
        };

        var result = JsonSerializer.Serialize(error, JsonSerializerOptions);

        await response.WriteAsJsonAsync(result);
    }
}
