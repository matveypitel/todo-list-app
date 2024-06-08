using System.ComponentModel.DataAnnotations;
using TodoListApp.Models.Enums;

namespace TodoListApp.Models.DTOs;
public class TaskItemModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Please enter the title")]
    [StringLength(100, ErrorMessage = "Title length can't be more than 100 symbols")]
    public string Title { get; set; } = string.Empty;

    [StringLength(150, ErrorMessage = "Description length can't be more than 150 symbols")]
    public string? Description { get; set; }

    public DateTime CreatedDate { get; init; } = DateTime.Now;

    public DateTime? DueDate { get; set; }

    public string? OwnerId { get; set; }

    public TaskItemStatus Status { get; set; } = TaskItemStatus.NotStarted;

    public string? Assignee { get; set; }

    public bool IsActive => this.Status == TaskItemStatus.NotStarted || this.Status == TaskItemStatus.InProgress;

    public bool IsOverDue => this.DueDate.HasValue && this.DueDate.Value.Date < DateTime.Now.Date;
}
