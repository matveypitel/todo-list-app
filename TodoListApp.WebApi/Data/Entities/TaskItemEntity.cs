using TodoListApp.Models.Enums;

namespace TodoListApp.WebApi.Data.Entities;

/// <summary>
/// Represents a task item entity.
/// </summary>
public class TaskItemEntity
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    public DateTime CreatedDate { get; init; } = DateTime.Now;

    public DateTime? DueDate { get; set; }

    public TaskItemStatus Status { get; set; } = TaskItemStatus.NotStarted;

    public string Owner { get; set; } = string.Empty;

    public string AssignedTo { get; set; } = null!;

    public int TodoListId { get; set; }

    public TodoListEntity TodoList { get; set; } = null!;

    public bool IsActive { get; set; }

    public bool IsOverDue { get; set; }

    public ICollection<TagEntity> Tags { get; init; } = new List<TagEntity>();

    public ICollection<CommentEntity> Comments { get; init; } = new List<CommentEntity>();
}
