namespace TodoListApp.Models.DTOs;
public class TagModel
{
    public int Id { get; set; }

    public string Label { get; set; } = string.Empty;

    public ICollection<TaskItemModel> Tasks { get; init; } = new List<TaskItemModel>();
}
