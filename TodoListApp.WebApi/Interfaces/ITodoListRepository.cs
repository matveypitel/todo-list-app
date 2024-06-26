using TodoListApp.Models.DTOs;
using TodoListApp.WebApi.Data.Entities;

namespace TodoListApp.WebApi.Interfaces;

/// <summary>
/// Represents a repository for managing to-do lists.
/// </summary>
public interface ITodoListRepository
{
    /// <summary>
    /// Retrieves a paged list of to-do lists for a specific user.
    /// </summary>
    /// <param name="userName">The username of the user.</param>
    /// <param name="page">The page number.</param>
    /// <param name="pageSize">The number of items per page.</param>
    /// <returns>A paged list of to-do lists.</returns>
    Task<PagedModel<TodoListEntity>> GetPagedListAsync(string userName, int page, int pageSize);

    /// <summary>
    /// Retrieves a to-do list by its ID for a specific user.
    /// </summary>
    /// <param name="id">The ID of the to-do list.</param>
    /// <param name="userName">The username of the user.</param>
    /// <returns>The to-do list entity.</returns>
    Task<TodoListEntity> GetByIdAsync(int id, string userName);

    /// <summary>
    /// Creates a new to-do list for a specific user.
    /// </summary>
    /// <param name="todoListEntity">The to-do list entity to create.</param>
    /// <param name="userName">The username of the user.</param>
    /// <returns>The created to-do list entity.</returns>
    Task<TodoListEntity> CreateAsync(TodoListEntity todoListEntity, string userName);

    /// <summary>
    /// Updates an existing to-do list for a specific user.
    /// </summary>
    /// <param name="id">The ID of the to-do list.</param>
    /// <param name="todoListEntity">The updated to-do list entity.</param>
    /// <param name="userName">The username of the user.</param>
    Task UpdateAsync(int id, TodoListEntity todoListEntity, string userName);

    /// <summary>
    /// Deletes a to-do list by its ID for a specific user.
    /// </summary>
    /// <param name="id">The ID of the to-do list.</param>
    /// <param name="userName">The username of the user.</param>
    Task DeleteAsync(int id, string userName);
}
