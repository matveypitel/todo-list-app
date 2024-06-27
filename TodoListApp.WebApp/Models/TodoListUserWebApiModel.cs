using TodoListApp.Models.Enums;

namespace TodoListApp.WebApp.Models;

/// <summary>
/// Represents a user in the TodoList web API.
/// </summary>
public class TodoListUserWebApiModel
{
    /// <summary>
    /// Gets or sets the username of the user.
    /// </summary>
    public string UserName { get; set; } = null!;

    /// <summary>
    /// Gets or sets the role of the user.
    /// </summary>
    public TodoListRole Role { get; set; } = TodoListRole.Viewer;
}
