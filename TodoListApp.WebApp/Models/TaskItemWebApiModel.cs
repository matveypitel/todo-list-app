using TodoListApp.Models.DTOs;
using TodoListApp.Models.Enums;

namespace TodoListApp.WebApp.Models;

/// <summary>
/// Represents a task item in the Web API model.
/// </summary>
public class TaskItemWebApiModel
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    public DateTime CreatedDate { get; init; } = DateTime.Now;

    public DateTime? DueDate { get; set; }

    public string? Owner { get; set; }

    public int TodoListId { get; set; }

    public TaskItemStatus Status { get; set; } = TaskItemStatus.NotStarted;

    public string? AssignedTo { get; set; }

    public bool IsActive => this.Status == TaskItemStatus.NotStarted || this.Status == TaskItemStatus.InProgress;

    public bool IsOverDue => this.DueDate.HasValue && this.DueDate.Value.Date < DateTime.Now.Date;

    public ICollection<TagWebApiModel> Tags { get; init; } = new List<TagWebApiModel>();

    public ICollection<CommentModel> Comments { get; init; } = new List<CommentModel>();
}
