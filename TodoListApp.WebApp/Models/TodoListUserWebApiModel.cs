using TodoListApp.Models.Enums;

namespace TodoListApp.WebApp.Models;

/// <summary>
/// Represents a user in the TodoList web API.
/// </summary>
public class TodoListUserWebApiModel
{
    public string UserName { get; set; } = null!;

    public TodoListRole Role { get; set; } = TodoListRole.Viewer;
}
