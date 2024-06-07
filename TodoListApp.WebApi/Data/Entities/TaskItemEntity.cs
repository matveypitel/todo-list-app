namespace TodoListApp.WebApi.Data.Entities;

public class TaskItemEntity
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    public DateTime CreatedDate { get; init; } = DateTime.Now;

    public DateTime? DueDate { get; set; }

    public string Status { get; set; } = "Not Started";

    public string OwnerId { get; set; } = string.Empty;

    public string Assignee { get; set; } = null!;

    public int TodoListId { get; set; }

    public TodoListEntity TodoList { get; set; } = null!;

    public bool IsActive => this.Status == "Not Started" || this.Status == "In Progress";
}
