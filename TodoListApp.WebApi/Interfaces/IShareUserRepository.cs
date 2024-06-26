using TodoListApp.Models.DTOs;
using TodoListApp.Models.Enums;
using TodoListApp.WebApi.Data.Entities;

namespace TodoListApp.WebApi.Interfaces;

/// <summary>
/// Represents a repository for managing shared users in a to-do list.
/// </summary>
public interface IShareUserRepository
{
    /// <summary>
    /// Retrieves a paged list of users associated with a to-do list.
    /// </summary>
    /// <param name="todoListId">The ID of the to-do list.</param>
    /// <param name="requesterUserName">The username of the requester.</param>
    /// <param name="page">The page number.</param>
    /// <param name="pageSize">The number of items per page.</param>
    /// <returns>A paged list of users.</returns>
    Task<PagedModel<TodoListUserEntity>> GetPagedListAsync(int todoListId, string requesterUserName, int page, int pageSize);

    /// <summary>
    /// Retrieves a user by their username associated with a to-do list.
    /// </summary>
    /// <param name="todoListId">The ID of the to-do list.</param>
    /// <param name="requesterUserName">The username of the requester.</param>
    /// <param name="userName">The username of the user to retrieve.</param>
    /// <returns>The user entity.</returns>
    Task<TodoListUserEntity> GetByNameAsync(int todoListId, string requesterUserName, string userName);

    /// <summary>
    /// Adds a user to a to-do list with the specified role.
    /// </summary>
    /// <param name="todoListId">The ID of the to-do list.</param>
    /// <param name="requesterUserName">The username of the requester.</param>
    /// <param name="userName">The username of the user to add.</param>
    /// <param name="role">The role of the user.</param>
    /// <returns>The added user entity.</returns>
    Task<TodoListUserEntity> AddUserToTodoListAsync(int todoListId, string requesterUserName, string userName, TodoListRole role);

    /// <summary>
    /// Updates the role of a user associated with a to-do list.
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
