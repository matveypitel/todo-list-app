namespace TodoListApp.Models.DTOs;
public class TaskItemModel
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    public DateTime? DueDate { get; set; }

    public string Status { get; set; } = "Not Started";

    public string Assignee { get; set; } = string.Empty;

    public bool IsActive => this.Status == "Not Started" || this.Status == "In Progress";
}
