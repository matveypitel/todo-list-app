using System.ComponentModel.DataAnnotations;
using TodoListApp.Models.Enums;
using TodoListApp.Models.ValidationAttributes;

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

    [DueDate(ErrorMessage = "Due date can't be earlier than now date")]
    public DateTime? DueDate { get; set; }

    public string? OwnerId { get; set; }

    public int TodoListId { get; set; }

    [Range(0, 2)]
    public TaskItemStatus Status { get; set; } = TaskItemStatus.NotStarted;

    public string? AssignedTo { get; set; }

    public bool IsActive => this.Status == TaskItemStatus.NotStarted || this.Status == TaskItemStatus.InProgress;

    public bool IsOverDue => this.DueDate.HasValue && this.DueDate.Value.Date < DateTime.Now.Date;
}
