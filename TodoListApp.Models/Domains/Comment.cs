namespace TodoListApp.Models.Domains;

/// <summary>
/// Represents a comment made on a task item.
/// </summary>
public class Comment
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
    /// Gets or sets the ID of the task associated with the comment.
    /// </summary>
    public int TaskId { get; set; }

    /// <summary>
    /// Gets or sets the task associated with the comment.
    /// </summary>
    public TaskItem Task { get; set; } = null!;
}
