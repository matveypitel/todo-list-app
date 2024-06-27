using System.ComponentModel.DataAnnotations;

namespace TodoListApp.Models.DTOs;

/// <summary>
/// Represents a data transfer object for a comment made on a task item.
/// </summary>
public class CommentModel
{
    /// <summary>
    /// Gets or sets the comment ID.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the comment text.
    /// </summary>
    [Required(ErrorMessage = "Please enter comment text")]
    [StringLength(200, ErrorMessage = "Comment text length can't be more than 40 symbols")]
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
