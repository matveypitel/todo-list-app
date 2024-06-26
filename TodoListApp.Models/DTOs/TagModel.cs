using System.ComponentModel.DataAnnotations;

namespace TodoListApp.Models.DTOs;

/// <summary>
/// Represents a data transfer object for a tag that can be associated with task items.
/// </summary>
public class TagModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Please enter a label")]
    [StringLength(40, ErrorMessage = "Tag label length can't be more than 40 symbols")]
    public string Label { get; set; } = string.Empty;

    public ICollection<TaskItemModel> Tasks { get; init; } = new List<TaskItemModel>();
}
