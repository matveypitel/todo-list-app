using TodoListApp.Models.Domains;
using TodoListApp.Models.DTOs;
using TodoListApp.Models.Enums;

namespace TodoListApp.WebApi.Interfaces;

/// <summary>
/// Represents a service for sharing user data in a to-do list.
/// </summary>
public interface IShareUserDatabaseService
{
    /// <summary>
    /// Retrieves a paged list of users in a to-do list.
    /// </summary>
    /// <param name="todoListId">The ID of the to-do list.</param>
    /// <param name="requesterUserName">The username of the requester.</param>
    /// <param name="page">The page number.</param>
    /// <param name="pageSize">The number of users per page.</param>
    /// <returns>A paged model of to-do list users.</returns>
    Task<PagedModel<TodoListUser>> GetPagedTodoListUsersListAsync(int todoListId, string requesterUserName, int page, int pageSize);

    /// <summary>
    /// Retrieves a user by their username in a to-do list.
    /// </summary>
    /// <param name="todoListId">The ID of the to-do list.</param>
    /// <param name="requesterUserName">The username of the requester.</param>
    /// <param name="userName">The username of the user to retrieve.</param>
    /// <returns>The to-do list user.</returns>
    Task<TodoListUser> GetUserByNameAsync(int todoListId, string requesterUserName, string userName);

    /// <summary>
    /// Adds a user to a to-do list.
    /// </summary>
    /// <param name="todoListId">The ID of the to-do list.</param>
    /// <param name="requesterUserName">The username of the requester.</param>
    /// <param name="userName">The username of the user to add.</param>
    /// <param name="role">The role of the user in the to-do list.</param>
    /// <returns>The added to-do list user.</returns>
    Task<TodoListUser> AddUserToTodoListAsync(int todoListId, string requesterUserName, string userName, TodoListRole role);

    /// <summary>
    /// Updates the role of a user in a to-do list.
    /// </summary>
    /// <param name="todoListId">The ID of the to-do list.</param>
    /// <param name="requesterUserName">The username of the requester.</param>
    /// <param name="userName">The username of the user to update.</param>
    /// <param name="newRole">The new role of the user.</param>
    Task UpdateUserRoleAsync(int todoListId, string requesterUserName, string userName, TodoListRole newRole);

    /// <summary>
    /// Removes a user from a to-do list.
    /// </summary>
    /// <param name="todoListId">The ID of the to-do list.</param>
    /// <param name="requesterUserName">The username of the requester.</param>
    /// <param name="userName">The username of the user to remove.</param>
    Task RemoveUserFromTodoListAsync(int todoListId, string requesterUserName, string userName);
}
