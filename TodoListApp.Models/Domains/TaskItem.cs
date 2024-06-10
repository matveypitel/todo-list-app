using TodoListApp.Models.Enums;

namespace TodoListApp.Models.Domains;
public class TaskItem
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    public DateTime CreatedDate { get; init; } = DateTime.Now;

    public DateTime? DueDate { get; set; }

    public TaskItemStatus Status { get; set; } = TaskItemStatus.NotStarted;

    public string OwnerId { get; set; } = string.Empty;

    public string? AssignedTo { get; set; }

    public int TodoListId { get; set; }

    public TodoList TodoList { get; set; } = null!;

    public bool IsActive => this.Status == TaskItemStatus.NotStarted || this.Status == TaskItemStatus.InProgress;

    public bool IsOverDue => this.DueDate.HasValue && this.DueDate.Value.Date < DateTime.Now.Date;
}
