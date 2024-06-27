namespace TodoListApp.WebApp.Models;

/// <summary>
/// Represents a CommentWebApiModel.
/// </summary>
public class CommentWebApiModel
{
    /// <summary>
    /// Gets or sets the ID of the comment.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the text of the comment.
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
}
