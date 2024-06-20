namespace TodoListApp.WebApp.Models;

public class CommentWebApiModel
{
    public int Id { get; set; }

    public string Text { get; set; } = string.Empty;

    public string Owner { get; set; } = string.Empty;

    public int TaskId { get; set; }
}
