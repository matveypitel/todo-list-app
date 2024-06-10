namespace TodoListApp.Models.Domains;
public class TodoList
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    public string Owner { get; set; } = string.Empty;

    public ICollection<TaskItem> Tasks { get; init; } = new List<TaskItem>();
}
