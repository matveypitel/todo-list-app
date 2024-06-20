using TodoListApp.Models.Domains;
using TodoListApp.Models.DTOs;

namespace TodoListApp.WebApi.Interfaces;

public interface ITodoListDatabaseService
{
    Task<PagedModel<TodoList>> GetPagedListOfTodoListsAsync(string userName, int page, int pageSize);

    Task<TodoList> GetTodoListByIdAsync(int id, string userName);

    Task<TodoList> CreateTodoListAsync(TodoList todoList, string userName);

    Task UpdateTodoListAsync(int id, TodoList todoList, string userName);

    Task DeleteTodoListAsync(int id, string userName);
}
