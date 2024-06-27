namespace TodoListApp.WebApp.Models.ViewModels;

/// <summary>
/// Represents an error view model in the TodoListApp web application.
/// </summary>
public class ErrorViewModel
{
    /// <summary>
    /// Gets or sets the request ID.
    /// </summary>
    public string? RequestId { get; set; }

    /// <summary>
    /// Gets a value indicating whether the request ID should be shown.
    /// </summary>
    public bool ShowRequestId => !string.IsNullOrEmpty(this.RequestId);
}
