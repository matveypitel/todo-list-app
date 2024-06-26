using TodoListApp.Models.Enums;

namespace TodoListApp.WebApi.Data.Entities;

/// <summary>
/// Represents a user entity in the TodoList application.
/// </summary>
public class TodoListUserEntity
{
    public string UserName { get; set; } = null!;

    public int TodoListId { get; set; }

    public TodoListRole Role { get; set; }

    public TodoListEntity TodoList { get; set; } = null!;
}
