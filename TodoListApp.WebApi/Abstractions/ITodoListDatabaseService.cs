using TodoListApp.Models.Domains;

namespace TodoListApp.WebApi.Abstractions;

public interface ITodoListDatabaseService
{
    Task<IEnumerable<TodoList>> GetListOfTodoListsAsync(string userId, int page, int pageSize);

    Task<TodoList> GetTodoListByIdAsync(int id, string userId);

    Task<TodoList> CreateTodoListAsync(TodoList todoList);

    Task UpdateTodoListAsync(int id, TodoList todoList);

    Task DeleteTodoListAsync(int id, string userId);

    Task<int> GetCountAsync(string userId);
}
