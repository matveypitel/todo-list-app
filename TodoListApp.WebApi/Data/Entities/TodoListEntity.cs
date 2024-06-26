namespace TodoListApp.WebApi.Data.Entities;

/// <summary>
/// Represents a to-do list entity.
/// </summary>
public class TodoListEntity
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    public ICollection<TaskItemEntity> Tasks { get; init; } = new List<TaskItemEntity>();

    public ICollection<TodoListUserEntity> Users { get; init; } = new List<TodoListUserEntity>();
}
