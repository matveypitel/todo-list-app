using System.ComponentModel.DataAnnotations;

namespace TodoListApp.Models.DTOs;
public class CommentModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Please enter comment text")]
    [StringLength(200, ErrorMessage = "Comment text length can't be more than 40 symbols")]
    public string Text { get; set; } = string.Empty;

    public string Owner { get; set; } = string.Empty;

    public int TaskId { get; set; }
}
