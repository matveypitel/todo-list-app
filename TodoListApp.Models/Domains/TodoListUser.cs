using TodoListApp.Models.Enums;

namespace TodoListApp.Models.Domains;

/// <summary>
/// Represents a user associated with a to-do list, including their role within that list.
/// </summary>
public class TodoListUser
{
    /// <summary>
    /// Gets or sets the username of the user.
    /// </summary>
    public string UserName { get; set; } = null!;

    /// <summary>
    /// Gets or sets the ID of the to-do list associated with the user.
    /// </summary>
    public int TodoListId { get; set; }

    /// <summary>
    /// Gets or sets the role of the user within the to-do list.
    /// </summary>
    public TodoListRole Role { get; set; }

    /// <summary>
    /// Gets or sets the to-do list associated with the user.
    /// </summary>
    public TodoList TodoList { get; set; } = null!;
}
