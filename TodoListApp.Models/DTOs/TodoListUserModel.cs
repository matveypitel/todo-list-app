using TodoListApp.Models.Enums;

namespace TodoListApp.Models.DTOs;

/// <summary>
/// Represents a data transfer object for a user associated with a to-do list.
/// </summary>
public class TodoListUserModel
{
    public string UserName { get; set; } = null!;

    public int? TodoListId { get; set; }

    public TodoListRole Role { get; set; }
}
