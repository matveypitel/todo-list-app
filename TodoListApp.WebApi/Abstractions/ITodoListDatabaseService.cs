using TodoListApp.Models.Domains;
using TodoListApp.Models.DTOs;

namespace TodoListApp.WebApi.Abstractions;

public interface ITodoListDatabaseService
{
    Task<PagedModel<TodoList>> GetPagedListOfTodoListsAsync(string userId, int page, int pageSize);

    Task<TodoList> GetTodoListByIdAsync(int id, string userId);

    Task<TodoList> CreateTodoListAsync(TodoList todoList);

    Task UpdateTodoListAsync(int id, TodoList todoList);

    Task DeleteTodoListAsync(int id, string userId);
}
