namespace TodoListApp.Models.Domains;
public class Tag
{
    public int Id { get; set; }

    public string Label { get; set; } = string.Empty;

    public ICollection<TaskItem> Tasks { get; init; } = new List<TaskItem>();
}
