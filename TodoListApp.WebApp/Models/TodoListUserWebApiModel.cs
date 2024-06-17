using TodoListApp.Models.Enums;

namespace TodoListApp.WebApp.Models;

public class TodoListUserWebApiModel
{
    public string UserName { get; set; } = null!;

    public TodoListRole Role { get; set; } = TodoListRole.Viewer;
}
