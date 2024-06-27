using TodoListApp.Models.Enums;

namespace TodoListApp.WebApi.Data.Entities;

/// <summary>
/// Represents a task item entity.
/// </summary>
public class TaskItemEntity
{
    /// <summary>
    /// Gets or sets the ID of the task item.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the title of the task item.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the description of the task item.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or initialize the created date of the task item.
    /// </summary>
    public DateTime CreatedDate { get; init; } = DateTime.Now;

    /// <summary>
    /// Gets or sets the due date of the task item.
    /// </summary>
    public DateTime? DueDate { get; set; }

    /// <summary>
    /// Gets or sets the status of the task item.
    /// </summary>
    public TaskItemStatus Status { get; set; } = TaskItemStatus.NotStarted;

    /// <summary>
    /// Gets or sets the owner of the task item.
    /// </summary>
    public string Owner { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the assigned to of the task item.
    /// </summary>
    public string AssignedTo { get; set; } = null!;

    /// <summary>
    /// Gets or sets the ID of the to-do list associated with the task item.
    /// </summary>
    public int TodoListId { get; set; }

    /// <summary>
    /// Gets or sets the to-do list associated with the task item.
    /// </summary>
    public TodoListEntity TodoList { get; set; } = null!;

    /// <summary>
    /// Gets or sets a value indicating whether the task item is active.
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the task item is overdue.
    /// </summary>
    public bool IsOverDue { get; set; }

    /// <summary>
    /// Gets or initialize the tags associated with the task item.
    /// </summary>
    public ICollection<TagEntity> Tags { get; init; } = new List<TagEntity>();

    /// <summary>
    /// Gets or initialize sets the comments associated with the task item.
    /// </summary>
    public ICollection<CommentEntity> Comments { get; init; } = new List<CommentEntity>();
}
