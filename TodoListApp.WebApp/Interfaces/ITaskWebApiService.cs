using TodoListApp.Models.Domains;
using TodoListApp.Models.DTOs;
using TodoListApp.Models.Enums;

namespace TodoListApp.WebApp.Interfaces;

/// <summary>
/// Represents a service for interacting with the Task Web API.
/// </summary>
public interface ITaskWebApiService
{
    /// <summary>
    /// Gets the user's role in a specific to-do list asynchronously.
    /// </summary>
    /// <param name="token">The user's authentication token.</param>
    /// <param name="todoListId">The ID of the to-do list.</param>
    /// <returns>The user's role in the to-do list.</returns>
    Task<TodoListRole> GetUserRoleInTodoListAsync(string token, int todoListId);

    /// <summary>
    /// Gets a paged list of tasks in a specific to-do list asynchronously.
    /// </summary>
    /// <param name="token">The user's authentication token.</param>
    /// <param name="todoListId">The ID of the to-do list.</param>
    /// <param name="page">The page number.</param>
    /// <param name="pageSize">The number of tasks per page.</param>
    /// <returns>A paged list of tasks.</returns>
    Task<PagedModel<TaskItem>> GetPagedTasksAsync(string token, int todoListId, int page, int pageSize);

    /// <summary>
    /// Gets a paged and searched list of tasks asynchronously.
    /// </summary>
    /// <param name="token">The user's authentication token.</param>
    /// <param name="page">The page number.</param>
    /// <param name="pageSize">The number of tasks per page.</param>
    /// <param name="title">The title of the task (optional).</param>
    /// <param name="creationDate">The creation date of the task (optional).</param>
    /// <param name="dueDate">The due date of the task (optional).</param>
    /// <returns>A paged and searched list of tasks.</returns>
    Task<PagedModel<TaskItem>> GetPagedSearchedTaskAsync(string token, int page, int pageSize, string? title, string? creationDate, string? dueDate);

    /// <summary>
    /// Gets a task by its ID and to-do list ID asynchronously.
    /// </summary>
    /// <param name="token">The user's authentication token.</param>
    /// <param name="id">The ID of the task.</param>
    /// <param name="todoListId">The ID of the to-do list.</param>
    /// <returns>The task with the specified ID and to-do list ID.</returns>
    Task<TaskItem> GetTaskByIdAsync(string token, int id, int todoListId);

    /// <summary>
    /// Creates a new task in a specific to-do list asynchronously.
    /// </summary>
    /// <param name="token">The user's authentication token.</param>
    /// <param name="todoListId">The ID of the to-do list.</param>
    /// <param name="task">The task to create.</param>
    /// <returns>The created task.</returns>
    Task<TaskItem> CreateTaskAsync(string token, int todoListId, TaskItem task);

    /// <summary>
    /// Updates a task in a specific to-do list asynchronously.
    /// </summary>
    /// <param name="token">The user's authentication token.</param>
    /// <param name="id">The ID of the task.</param>
    /// <param name="todoListId">The ID of the to-do list.</param>
    /// <param name="task">The updated task.</param>
    Task UpdateTaskAsync(string token, int id, int todoListId, TaskItem task);

    /// <summary>
    /// Deletes a task from a specific to-do list asynchronously.
    /// </summary>
    /// <param name="token">The user's authentication token.</param>
    /// <param name="id">The ID of the task.</param>
    /// <param name="todoListId">The ID of the to-do list.</param>
    Task DeleteTaskAsync(string token, int id, int todoListId);

    /// <summary>
    /// Gets a paged list of tasks assigned to the user asynchronously.
    /// </summary>
    /// <param name="token">The user's authentication token.</param>
    /// <param name="page">The page number.</param>
    /// <param name="pageSize">The number of tasks per page.</param>
    /// <param name="status">The status of the tasks (optional).</param>
    /// <param name="sort">The sort order of the tasks (optional).</param>
    /// <returns>A paged list of tasks assigned to the user.</returns>
    Task<PagedModel<TaskItem>> GetAssignedTasksToUserAsync(string token, int page, int pageSize, string? status, string? sort);

    /// <summary>
    /// Gets a task assigned to the user by its ID asynchronously.
    /// </summary>
    /// <param name="token">The user's authentication token.</param>
    /// <param name="id">The ID of the task.</param>
    /// <returns>The task assigned to the user with the specified ID.</returns>
    Task<TaskItem> GetAssignedTaskByIdAsync(string token, int id);

    /// <summary>
    /// Updates the status of a task asynchronously.
    /// </summary>
    /// <param name="token">The user's authentication token.</param>
    /// <param name="id">The ID of the task.</param>
    /// <param name="task">The updated task.</param>
    Task UpdateTaskStatusAsync(string token, int id, TaskItem task);
}
