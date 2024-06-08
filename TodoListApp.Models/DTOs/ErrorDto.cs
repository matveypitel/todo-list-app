using System.Net;

namespace TodoListApp.Models.DTOs;
public class ErrorDto
{
    public int StatusCode { get; set; } = HttpStatusCode.InternalServerError.GetHashCode();

    public string Message { get; set; } = string.Empty;
}
