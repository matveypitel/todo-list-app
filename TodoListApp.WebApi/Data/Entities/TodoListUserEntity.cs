using TodoListApp.Models.Enums;

namespace TodoListApp.WebApi.Data.Entities;

/// <summary>
/// Represents a user entity in the TodoList application.
/// </summary>
public class TodoListUserEntity
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
    /// Gets or sets the role of the user in the to-do list.
    /// </summary>
    public TodoListRole Role { get; set; }

    /// <summary>
    /// Gets or sets the to-do list associated with the user.
    /// </summary>
    public TodoListEntity TodoList { get; set; } = null!;
}
