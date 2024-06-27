namespace TodoListApp.WebApi.Data.Entities;

/// <summary>
/// Represents a comment entity.
/// </summary>
public class CommentEntity
{
    /// <summary>
    /// Gets or sets the comment ID.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the comment text.
    /// </summary>
    public string Text { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the owner of the comment.
    /// </summary>
    public string Owner { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the ID of the associated task.
    /// </summary>
    public int TaskId { get; set; }

    /// <summary>
    /// Gets or sets the associated task.
    /// </summary>
    public TaskItemEntity Task { get; set; } = null!;
}
