using TodoListApp.Models.Domains;
using TodoListApp.Models.DTOs;

namespace TodoListApp.WebApp.Interfaces;

public interface ITodoListWebApiService
{
    Task<PagedModel<TodoList>> GetPagedListOfTodoListsAsync(string token, int page, int pageSize);

    Task<TodoList> GetTodoListByIdAsync(string token, int id);

    Task<TodoList> CreateTodoListAsync(string token, TodoList todoList);

    Task UpdateTodoListAsync(string token, int id, TodoList todoList);

    Task DeleteTodoListAsync(string token, int id);
}
