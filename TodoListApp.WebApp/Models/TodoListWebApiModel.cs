namespace TodoListApp.WebApp.Models;

/// <summary>
/// Represents a To-do List in the Web API.
/// </summary>
public class TodoListWebApiModel
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    public ICollection<TodoListUserWebApiModel> Users { get; init; } = new List<TodoListUserWebApiModel>();
}
