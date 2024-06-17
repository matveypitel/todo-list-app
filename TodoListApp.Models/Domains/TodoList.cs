namespace TodoListApp.Models.Domains;
public class TodoList
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    public ICollection<TaskItem> Tasks { get; init; } = new List<TaskItem>();

    public ICollection<TodoListUser> Users { get; init; } = new List<TodoListUser>();
}
