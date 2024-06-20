namespace TodoListApp.Models.Domains;
public class Comment
{
    public int Id { get; set; }

    public string Text { get; set; } = string.Empty;

    public string Owner { get; set; } = string.Empty;

    public int TaskId { get; set; }

    public TaskItem Task { get; set; } = null!;
}
