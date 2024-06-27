namespace TodoListApp.WebApi.Data.Entities;

/// <summary>
/// Represents a tag entity.
/// </summary>
public class TagEntity
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
    /// Gets or sets the collection of task items associated with the tag.
    /// </summary>
    public ICollection<TaskItemEntity> Tasks { get; init; } = new List<TaskItemEntity>();
}
