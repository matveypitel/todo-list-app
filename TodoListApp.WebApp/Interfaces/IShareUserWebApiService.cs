using TodoListApp.Models.Domains;
using TodoListApp.Models.DTOs;

namespace TodoListApp.WebApp.Interfaces;

/// <summary>
/// Represents a service for sharing users in a to-do list through a Web API.
/// </summary>
public interface IShareUserWebApiService
{
    /// <summary>
    /// Retrieves a paged list of users in a to-do list.
    /// </summary>
    /// <param name="token">The authentication token.</param>
    /// <param name="todoListId">The ID of the to-do list.</param>
    /// <param name="page">The page number.</param>
    /// <param name="pageSize">The number of items per page.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the paged list of users.</returns>
    Task<PagedModel<TodoListUser>> GetPagedListOfUsersInTodoListAsync(string token, int todoListId, int page, int pageSize);

    /// <summary>
    /// Retrieves a user in a to-do list by their username.
    /// </summary>
    /// <param name="token">The authentication token.</param>
    /// <param name="todoListId">The ID of the to-do list.</param>
    /// <param name="userName">The username of the user.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the user.</returns>
    Task<TodoListUser> GetUserInTodoListAsync(string token, int todoListId, string userName);

    /// <summary>
    /// Adds a user to a to-do list.
    /// </summary>
    /// <param name="token">The authentication token.</param>
    /// <param name="todoListId">The ID of the to-do list.</param>
    /// <param name="user">The user to add.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the added user.</returns>
    Task<TodoListUser> AddUserToTodoListAsync(string token, int todoListId, TodoListUser user);

    /// <summary>
    /// Updates the role of a user in a to-do list.
    /// </summary>
    /// <param name="token">The authentication token.</param>
    /// <param name="todoListId">The ID of the to-do list.</param>
    /// <param name="userName">The username of the user.</param>
    /// <param name="user">The updated user.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task UpdateUserRoleAsync(string token, int todoListId, string userName, TodoListUser user);

    /// <summary>
    /// Removes a user from a to-do list.
    /// </summary>
    /// <param name="token">The authentication token.</param>
    /// <param name="todoListId">The ID of the to-do list.</param>
    /// <param name="userName">The username of the user.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task RemoveUserFromTodoListAsync(string token, int todoListId, string userName);
}
