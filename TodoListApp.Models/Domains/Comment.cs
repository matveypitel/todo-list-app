namespace TodoListApp.Models.Domains;

/// <summary>
/// Represents a comment made on a task item.
/// </summary>
public class Comment
{
    public int Id { get; set; }

    public string Text { get; set; } = string.Empty;

    public string Owner { get; set; } = string.Empty;

    public int TaskId { get; set; }

    public TaskItem Task { get; set; } = null!;
}
