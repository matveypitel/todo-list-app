using System.ComponentModel.DataAnnotations;

namespace TodoListApp.Models.DTOs;
public class TaskItemModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Please enter the title")]
    [StringLength(100, ErrorMessage = "Title length can't be more than 100 symbols")]
    public string Title { get; set; } = string.Empty;

    [StringLength(150, ErrorMessage = "Description length can't be more than 150 symbols")]
    public string? Description { get; set; }

    public DateTime CreatedDate { get; } = DateTime.UtcNow;

    public DateTime? DueDate { get; set; }

    public string Status { get; set; } = "Not Started";

    public int? TodoListId { get; set; }

    public string? Assignee { get; set; }

    public bool IsActive => this.Status == "Not Started" || this.Status == "In Progress";
}
