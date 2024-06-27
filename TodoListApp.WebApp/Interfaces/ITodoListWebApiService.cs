using TodoListApp.Models.Domains;
using TodoListApp.Models.DTOs;

namespace TodoListApp.WebApp.Interfaces;

/// <summary>
/// Represents a web API service for managing to-do lists.
/// </summary>
public interface ITodoListWebApiService
{
    /// <summary>
    /// Retrieves a paged list of to-do lists.
    /// </summary>
    /// <param name="token">The authentication token.</param>
    /// <param name="page">The page number.</param>
    /// <param name="pageSize">The number of items per page.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the paged list of to-do lists.</returns>
    Task<PagedModel<TodoList>> GetPagedListOfTodoListsAsync(string token, int page, int pageSize);

    /// <summary>
    /// Retrieves a to-do list by its ID.
    /// </summary>
    /// <param name="token">The authentication token.</param>
    /// <param name="id">The ID of the to-do list.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the to-do list.</returns>
    Task<TodoList> GetTodoListByIdAsync(string token, int id);

    /// <summary>
    /// Creates a new to-do list.
    /// </summary>
    /// <param name="token">The authentication token.</param>
    /// <param name="todoList">The to-do list to create.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the created to-do list.</returns>
    Task<TodoList> CreateTodoListAsync(string token, TodoList todoList);

    /// <summary>
    /// Updates an existing to-do list.
    /// </summary>
    /// <param name="token">The authentication token.</param>
    /// <param name="id">The ID of the to-do list to update.</param>
    /// <param name="todoList">The updated to-do list.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task UpdateTodoListAsync(string token, int id, TodoList todoList);

    /// <summary>
    /// Deletes a to-do list by its ID.
    /// </summary>
    /// <param name="token">The authentication token.</param>
    /// <param name="id">The ID of the to-do list to delete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task DeleteTodoListAsync(string token, int id);
}
