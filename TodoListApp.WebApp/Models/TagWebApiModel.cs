using TodoListApp.Models.DTOs;

namespace TodoListApp.WebApp.Models;

/// <summary>
/// Represents a TagWebApiModel.
/// </summary>
public class TagWebApiModel
{
    /// <summary>
    /// Gets or sets the ID of the tag.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the label of the tag.
    /// </summary>
    public string Label { get; set; } = string.Empty;

    /// <summary>
    /// Gets or initialize the collection of task items associated with the tag.
    /// </summary>
    public ICollection<TaskItemModel> Tasks { get; init; } = new List<TaskItemModel>();
}
