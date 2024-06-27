namespace TodoListApp.WebApi.Data.Entities;

/// <summary>
/// Represents a to-do list entity.
/// </summary>
public class TodoListEntity
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
    /// Gets or sets the collection of task items associated with the to-do list.
    /// </summary>
    public ICollection<TaskItemEntity> Tasks { get; init; } = new List<TaskItemEntity>();

    /// <summary>
    /// Gets or sets the collection of users associated with the to-do list.
    /// </summary>
    public ICollection<TodoListUserEntity> Users { get; init; } = new List<TodoListUserEntity>();
}
