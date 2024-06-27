namespace TodoListApp.Models.Domains;

/// <summary>
/// Represents a to-do list with a collection of tasks.
/// </summary>
public class TodoList
{
    /// <summary>
    /// Gets or sets the ID of the to-do list.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the title of the to-do list.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the description of the to-do list.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or initializes the collection of tasks in the to-do list.
    /// </summary>
    public ICollection<TaskItem> Tasks { get; init; } = new List<TaskItem>();

    /// <summary>
    /// Gets or initializes the collection of users associated with the to-do list.
    /// </summary>
    public ICollection<TodoListUser> Users { get; init; } = new List<TodoListUser>();
}
