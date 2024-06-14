namespace TodoListApp.WebApi.Data.Entities;

public class TagEntity
{
    public int Id { get; set; }

    public string Label { get; set; } = string.Empty;

    public ICollection<TaskItemEntity> Tasks { get; init; } = new List<TaskItemEntity>();
}
