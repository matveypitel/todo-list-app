using TodoListApp.Models.Enums;

namespace TodoListApp.WebApi.Data.Entities;

public class TaskItemEntity
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    public DateTime CreatedDate { get; init; } = DateTime.Now;

    public DateTime? DueDate { get; set; }

    public TaskItemStatus Status { get; set; } = TaskItemStatus.NotStarted;

    public string OwnerId { get; set; } = string.Empty;

    public string AssignedTo { get; set; } = null!;

    public int TodoListId { get; set; }

    public TodoListEntity TodoList { get; set; } = null!;

    public bool IsActive { get; set; }

    public bool IsOverDue { get; set; }
}
