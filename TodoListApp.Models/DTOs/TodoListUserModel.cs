using TodoListApp.Models.Enums;

namespace TodoListApp.Models.DTOs;

/// <summary>
/// Represents a data transfer object for a user associated with a to-do list.
/// </summary>
public class TodoListUserModel
{
    /// <summary>
    /// Gets or sets the user name.
    /// </summary>
    public string UserName { get; set; } = null!;

    /// <summary>
    /// Gets or sets the ID of the to-do list associated with the user.
    /// </summary>
    public int? TodoListId { get; set; }

    /// <summary>
    /// Gets or sets the role of the user in the to-do list.
    /// </summary>
    public TodoListRole Role { get; set; }
}
