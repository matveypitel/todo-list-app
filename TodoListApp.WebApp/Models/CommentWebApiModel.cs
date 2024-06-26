namespace TodoListApp.WebApp.Models;

/// <summary>
/// Represents a CommentWebApiModel.
/// </summary>
public class CommentWebApiModel
{
    public int Id { get; set; }

    public string Text { get; set; } = string.Empty;

    public string Owner { get; set; } = string.Empty;

    public int TaskId { get; set; }
}
