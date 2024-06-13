using TodoListApp.Models.Domains;
using TodoListApp.Models.DTOs;

namespace TodoListApp.WebApi.Interfaces;

public interface ITodoListDatabaseService
{
    Task<PagedModel<TodoList>> GetPagedListOfTodoListsAsync(string ownerName, int page, int pageSize);

    Task<TodoList> GetTodoListByIdAsync(int id, string ownerName);

    Task<TodoList> CreateTodoListAsync(TodoList todoList);

    Task UpdateTodoListAsync(int id, TodoList todoList);

    Task DeleteTodoListAsync(int id, string ownerName);
}
