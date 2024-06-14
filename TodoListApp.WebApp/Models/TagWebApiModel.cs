using TodoListApp.Models.DTOs;

namespace TodoListApp.WebApp.Models;

public class TagWebApiModel
{
    public int Id { get; set; }

    public string Label { get; set; } = string.Empty;

    public ICollection<TaskItemModel> Tasks { get; init; } = new List<TaskItemModel>();
}
