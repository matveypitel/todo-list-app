using System.ComponentModel.DataAnnotations;

namespace TodoListApp.Models.DTOs;

/// <summary>
/// Represents a data transfer object for a tag that can be associated with task items.
/// </summary>
public class TagModel
{
    /// <summary>
    /// Gets or sets the ID of the tag.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the label of the tag.
    /// </summary>
    [Required(ErrorMessage = "Please enter a label")]
    [StringLength(40, ErrorMessage = "Tag label length can't be more than 40 symbols")]
    public string Label { get; set; } = string.Empty;

    /// <summary>
    /// Gets or initialize the collection of task items associated with the tag.
    /// </summary>
    public ICollection<TaskItemModel> Tasks { get; init; } = new List<TaskItemModel>();
}
