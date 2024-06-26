using System.ComponentModel.DataAnnotations;

namespace TodoListApp.Models.DTOs;

/// <summary>
/// Represents a data transfer object for a to-do list.
/// </summary>
public class TodoListModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Please enter the title")]
    [StringLength(100, ErrorMessage = "Title length can't be more than 100 symbols")]
    public string Title { get; set; } = string.Empty;

    [StringLength(150, ErrorMessage = "Description length can't be more than 150 symbols")]
    public string? Description { get; set; }

    public ICollection<TodoListUserModel> Users { get; init; } = new List<TodoListUserModel>();
}
