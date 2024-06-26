namespace TodoListApp.Models.Domains;

/// <summary>
/// Represents a tag that can be associated with multiple task items.
/// </summary>
public class Tag
{
    public int Id { get; set; }

    public string Label { get; set; } = string.Empty;

    public ICollection<TaskItem> Tasks { get; init; } = new List<TaskItem>();
}
