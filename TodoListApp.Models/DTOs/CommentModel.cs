using System.ComponentModel.DataAnnotations;

namespace TodoListApp.Models.DTOs;

/// <summary>
/// Represents a data transfer object for a comment made on a task item.
/// </summary>
public class CommentModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Please enter comment text")]
    [StringLength(200, ErrorMessage = "Comment text length can't be more than 40 symbols")]
    public string Text { get; set; } = string.Empty;

    public string Owner { get; set; } = string.Empty;

    public int TaskId { get; set; }
}
