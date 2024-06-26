using TodoListApp.Models.Domains;
using TodoListApp.Models.DTOs;

namespace TodoListApp.WebApi.Interfaces;

/// <summary>
/// Represents a service for interacting with the TodoList database.
/// </summary>
public interface ITodoListDatabaseService
{
    /// <summary>
    /// Retrieves a paged list of TodoLists for a specific user.
    /// </summary>
    /// <param name="userName">The username of the user.</param>
    /// <param name="page">The page number.</param>
    /// <param name="pageSize">The number of items per page.</param>
    /// <returns>A paged list of TodoLists.</returns>
    Task<PagedModel<TodoList>> GetPagedListOfTodoListsAsync(string userName, int page, int pageSize);

    /// <summary>
    /// Retrieves a TodoList by its ID for a specific user.
    /// </summary>
    /// <param name="id">The ID of the TodoList.</param>
    /// <param name="userName">The username of the user.</param>
    /// <returns>The TodoList with the specified ID.</returns>
    Task<TodoList> GetTodoListByIdAsync(int id, string userName);

    /// <summary>
    /// Creates a new TodoList for a specific user.
    /// </summary>
    /// <param name="todoList">The TodoList to create.</param>
    /// <param name="userName">The username of the user.</param>
    /// <returns>The created TodoList.</returns>
    Task<TodoList> CreateTodoListAsync(TodoList todoList, string userName);

    /// <summary>
    /// Updates an existing TodoList for a specific user.
    /// </summary>
    /// <param name="id">The ID of the TodoList to update.</param>
    /// <param name="todoList">The updated TodoList.</param>
    /// <param name="userName">The username of the user.</param>
    Task UpdateTodoListAsync(int id, TodoList todoList, string userName);

    /// <summary>
    /// Deletes a TodoList by its ID for a specific user.
    /// </summary>
    /// <param name="id">The ID of the TodoList to delete.</param>
    /// <param name="userName">The username of the user.</param>
    Task DeleteTodoListAsync(int id, string userName);
}
