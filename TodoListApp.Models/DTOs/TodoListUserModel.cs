using TodoListApp.Models.Enums;

namespace TodoListApp.Models.DTOs;

public class TodoListUserModel
{
    public string UserName { get; set; } = null!;

    public int? TodoListId { get; set; }

    public TodoListRole Role { get; set; }
}
