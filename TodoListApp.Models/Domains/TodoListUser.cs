using TodoListApp.Models.Enums;

namespace TodoListApp.Models.Domains;

/// <summary>
/// Represents a user associated with a to-do list, including their role within that list.
/// </summary>
public class TodoListUser
{
    public string UserName { get; set; } = null!;

    public int TodoListId { get; set; }

    public TodoListRole Role { get; set; }

    public TodoList TodoList { get; set; } = null!;
}
