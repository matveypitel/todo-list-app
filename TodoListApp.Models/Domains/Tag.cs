namespace TodoListApp.Models.Domains;

/// <summary>
/// Represents a tag that can be associated with multiple task items.
/// </summary>
public class Tag
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
    /// Gets or initializes the collection of task items associated with the tag.
    /// </summary>
    public ICollection<TaskItem> Tasks { get; init; } = new List<TaskItem>();
}
