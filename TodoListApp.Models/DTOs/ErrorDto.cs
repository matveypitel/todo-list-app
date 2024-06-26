using System.Net;

namespace TodoListApp.Models.DTOs;

/// <summary>
/// Represents a data transfer object for an error.
/// </summary>
public class ErrorDto
{
    public int StatusCode { get; set; } = HttpStatusCode.InternalServerError.GetHashCode();

    public string Message { get; set; } = string.Empty;
}
