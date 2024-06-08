using System.Net;
using System.Text.Json;
using TodoListApp.Models.DTOs;

namespace TodoListApp.WebApi.Middleware;

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

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        this.next = next;
        this.logger = logger;
    }

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
        catch (Exception ex) when (ex is not KeyNotFoundException && ex is not ArgumentNullException)
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
