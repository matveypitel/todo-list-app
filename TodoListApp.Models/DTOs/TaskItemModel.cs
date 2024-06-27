using System.ComponentModel.DataAnnotations;
using TodoListApp.Models.Enums;
using TodoListApp.Models.ValidationAttributes;

namespace TodoListApp.Models.DTOs;

/// <summary>
/// Represents a data transfer object for a task item within a to-do list.
/// </summary>
public class TaskItemModel
{
    /// <summary>
    /// Gets or sets the ID of the task item.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the title of the task item.
    /// </summary>
    [Required(ErrorMessage = "Please enter the title")]
    [StringLength(100, ErrorMessage = "Title length can't be more than 100 symbols")]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the description of the task item.
    /// </summary>
    [StringLength(150, ErrorMessage = "Description length can't be more than 150 symbols")]
    public string? Description { get; set; }

    /// <summary>
    /// Gets the created date of the task item.
    /// </summary>
    public DateTime CreatedDate { get; init; } = DateTime.Now;

    /// <summary>
    /// Gets or sets the due date of the task item.
    /// </summary>
    [DueDate(ErrorMessage = "Due date can't be earlier than now date")]
    public DateTime? DueDate { get; set; }

    /// <summary>
    /// Gets or sets the owner of the task item.
    /// </summary>
    public string? Owner { get; set; }

    /// <summary>
    /// Gets or sets the ID of the to-do list that the task item belongs to.
    /// </summary>
    public int TodoListId { get; set; }

    /// <summary>
    /// Gets or sets the status of the task item.
    /// </summary>
    public TaskItemStatus Status { get; set; } = TaskItemStatus.NotStarted;

    /// <summary>
    /// Gets or sets the assigned to user of the task item.
    /// </summary>
    public string? AssignedTo { get; set; }

    /// <summary>
    /// Gets a value indicating whether the task item is active.
    /// </summary>
    public bool IsActive => this.Status == TaskItemStatus.NotStarted || this.Status == TaskItemStatus.InProgress;

    /// <summary>
    /// Gets a value indicating whether the task item is overdue.
    /// </summary>
    public bool IsOverDue => this.DueDate.HasValue && this.DueDate.Value.Date < DateTime.Now.Date;

    /// <summary>
    /// Gets or initialize the tags associated with the task item.
    /// </summary>
    public ICollection<TagModel> Tags { get; init; } = new List<TagModel>();

    /// <summary>
    /// Gets or initialize the comments associated with the task item.
    /// </summary>
    public ICollection<CommentModel> Comments { get; init; } = new List<CommentModel>();
}
