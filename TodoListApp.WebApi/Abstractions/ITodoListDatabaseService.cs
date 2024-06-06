using TodoListApp.Models.Domains;

namespace TodoListApp.WebApi.Abstractions;

public interface ITodoListDatabaseService
{
    Task<IEnumerable<TodoList>> GetAllAsync(string userId, int pageNumber, int pageSize);

    Task<TodoList> GetByIdAsync(int id, string userId);

    Task<TodoList> CreateAsync(TodoList todoList);

    Task UpdateAsync(int id, TodoList todoList);

    Task DeleteAsync(int id, string userId);

    Task<int> GetCountAsync(string userId);
}
