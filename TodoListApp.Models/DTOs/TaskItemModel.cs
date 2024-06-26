using System.ComponentModel.DataAnnotations;
using TodoListApp.Models.Enums;
using TodoListApp.Models.ValidationAttributes;

namespace TodoListApp.Models.DTOs;

/// <summary>
/// Represents a data transfer object for a task item within a todo list.
/// </summary>
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

    public string? Owner { get; set; }

    public int TodoListId { get; set; }

    public TaskItemStatus Status { get; set; } = TaskItemStatus.NotStarted;

    public string? AssignedTo { get; set; }

    public bool IsActive => this.Status == TaskItemStatus.NotStarted || this.Status == TaskItemStatus.InProgress;

    public bool IsOverDue => this.DueDate.HasValue && this.DueDate.Value.Date < DateTime.Now.Date;

    public ICollection<TagModel> Tags { get; init; } = new List<TagModel>();

    public ICollection<CommentModel> Comments { get; init; } = new List<CommentModel>();
}
