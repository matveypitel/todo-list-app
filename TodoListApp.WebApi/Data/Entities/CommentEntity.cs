namespace TodoListApp.WebApi.Data.Entities;

/// <summary>
/// Represents a comment entity.
/// </summary>
public class CommentEntity
{
    public int Id { get; set; }

    public string Text { get; set; } = string.Empty;

    public string Owner { get; set; } = string.Empty;

    public int TaskId { get; set; }

    public TaskItemEntity Task { get; set; } = null!;
}
