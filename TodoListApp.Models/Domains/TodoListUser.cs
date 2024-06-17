using TodoListApp.Models.Enums;

namespace TodoListApp.Models.Domains;
public class TodoListUser
{
    public string UserName { get; set; } = null!;

    public int TodoListId { get; set; }

    public TodoListRole Role { get; set; }

    public TodoList TodoList { get; set; } = null!;
}
