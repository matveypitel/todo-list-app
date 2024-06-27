namespace TodoListApp.WebApp.Models;

/// <summary>
/// Represents a To-do List in the Web API.
/// </summary>
public class TodoListWebApiModel
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
    /// Gets or initialize the collection of users associated with the to-do list.
    /// </summary>
    public ICollection<TodoListUserWebApiModel> Users { get; init; } = new List<TodoListUserWebApiModel>();
}
