using System.ComponentModel.DataAnnotations;

namespace TodoListApp.Models.DTOs;

/// <summary>
/// Represents a data transfer object for a to-do list.
/// </summary>
public class TodoListModel
{
    /// <summary>
    /// Gets or sets the ID of the to-do list.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the title of the to-do list.
    /// </summary>
    [Required(ErrorMessage = "Please enter the title")]
    [StringLength(100, ErrorMessage = "Title length can't be more than 100 symbols")]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the description of the to-do list.
    /// </summary>
    [StringLength(150, ErrorMessage = "Description length can't be more than 150 symbols")]
    public string? Description { get; set; }

    /// <summary>
    /// Gets or initialize the users associated with the to-do list.
    /// </summary>
    public ICollection<TodoListUserModel> Users { get; init; } = new List<TodoListUserModel>();
}
