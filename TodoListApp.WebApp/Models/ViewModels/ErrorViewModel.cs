namespace TodoListApp.WebApp.Models.ViewModels;

/// <summary>
/// Represents an error view model in the TodoListApp web application.
/// </summary>
public class ErrorViewModel
{
    public string? RequestId { get; set; }

    public bool ShowRequestId => !string.IsNullOrEmpty(this.RequestId);
}
