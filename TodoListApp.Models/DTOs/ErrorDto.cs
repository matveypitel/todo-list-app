using System.Net;

namespace TodoListApp.Models.DTOs;

/// <summary>
/// Represents a data transfer object for an error.
/// </summary>
public class ErrorDto
{
    /// <summary>
    /// Gets or sets the status code of the error.
    /// </summary>
    public int StatusCode { get; set; } = HttpStatusCode.InternalServerError.GetHashCode();

    /// <summary>
    /// Gets or sets the message of the error.
    /// </summary>
    public string Message { get; set; } = string.Empty;
}
