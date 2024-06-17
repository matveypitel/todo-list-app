namespace TodoListApp.WebApi.Data.Entities;

public class TodoListEntity
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    public ICollection<TaskItemEntity> Tasks { get; } = new List<TaskItemEntity>();

    public ICollection<TodoListUserEntity> Users { get; } = new List<TodoListUserEntity>();
}
