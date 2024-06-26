using TodoListApp.Models.DTOs;
using TodoListApp.Models.Enums;
using TodoListApp.WebApi.Data.Entities;

namespace TodoListApp.WebApi.Interfaces;

/// <summary>
/// Represents a repository for managing tasks.
/// </summary>
public interface ITaskRepository
{
    /// <summary>
    /// Gets the user's role in a to-do list asynchronously.
    /// </summary>
    /// <param name="todoListId">The ID of the to-do list.</param>
    /// <param name="userName">The username of the user.</param>
    /// <returns>The user's role in the to-do list.</returns>
    Task<TodoListRole> GetUserRoleInTodoListAsync(int todoListId, string userName);

    /// <summary>
    /// Gets a task by ID and to-do list ID asynchronously.
    /// </summary>
    /// <param name="id">The ID of the task.</param>
    /// <param name="todoListId">The ID of the to-do list.</param>
    /// <param name="userName">The username of the user.</param>
    /// <returns>The task with the specified ID and to-do list ID.</returns>
    Task<TaskItemEntity> GetByIdAsync(int id, int todoListId, string userName);

    /// <summary>
    /// Gets an assigned task by ID asynchronously.
    /// </summary>
    /// <param name="id">The ID of the task.</param>
    /// <param name="userName">The username of the user.</param>
    /// <returns>The assigned task with the specified ID.</returns>
    Task<TaskItemEntity> GetAssignedByIdAsync(int id, string userName);

    /// <summary>
    /// Gets a paged list of tasks for a to-do list asynchronously.
    /// </summary>
    /// <param name="todoListId">The ID of the to-do list.</param>
    /// <param name="userName">The username of the user.</param>
    /// <param name="page">The page number.</param>
    /// <param name="pageSize">The number of tasks per page.</param>
    /// <returns>A paged list of tasks for the to-do list.</returns>
    Task<PagedModel<TaskItemEntity>> GetPagedListAsync(int todoListId, string userName, int page, int pageSize);

    /// <summary>
    /// Gets a paged list of tasks with a specific tag asynchronously.
    /// </summary>
    /// <param name="userName">The username of the user.</param>
    /// <param name="tagLabel">The label of the tag.</param>
    /// <param name="page">The page number.</param>
    /// <param name="pageSize">The number of tasks per page.</param>
    /// <returns>A paged list of tasks with the specified tag.</returns>
    Task<PagedModel<TaskItemEntity>> GetPagedListWithTagAsync(string userName, string tagLabel, int page, int pageSize);

    /// <summary>
    /// Gets a paged list of tasks assigned to a user asynchronously.
    /// </summary>
    /// <param name="userName">The username of the user.</param>
    /// <param name="page">The page number.</param>
    /// <param name="pageSize">The number of tasks per page.</param>
    /// <param name="status">The status of the tasks.</param>
    /// <param name="sort">The sort order of the tasks.</param>
    /// <returns>A paged list of tasks assigned to the user.</returns>
    Task<PagedModel<TaskItemEntity>> GetPagedListOfAssignedToUserAsync(string userName, int page, int pageSize, string? status, string? sort);

    /// <summary>
    /// Gets a paged list of tasks based on search criteria asynchronously.
    /// </summary>
    /// <param name="userName">The username of the user.</param>
    /// <param name="title">The title of the tasks.</param>
    /// <param name="creationDate">The creation date of the tasks.</param>
    /// <param name="dueDate">The due date of the tasks.</param>
    /// <param name="page">The page number.</param>
    /// <param name="pageSize">The number of tasks per page.</param>
    /// <returns>A paged list of tasks based on the search criteria.</returns>
    Task<PagedModel<TaskItemEntity>> GetPagedListOfSearchResultsAsync(string userName, string? title, string? creationDate, string? dueDate, int page, int pageSize);

    /// <summary>
    /// Creates a new task asynchronously.
    /// </summary>
    /// <param name="taskItemEntity">The task item entity.</param>
    /// <param name="userName">The username of the user.</param>
    /// <returns>The created task.</returns>
    Task<TaskItemEntity> CreateAsync(TaskItemEntity taskItemEntity, string userName);

    /// <summary>
    /// Updates a task asynchronously.
    /// </summary>
    /// <param name="id">The ID of the task.</param>
    /// <param name="todoListId">The ID of the to-do list.</param>
    /// <param name="taskItemEntity">The updated task item entity.</param>
    /// <param name="userName">The username of the user.</param>
    Task UpdateAsync(int id, int todoListId, TaskItemEntity taskItemEntity, string userName);

    /// <summary>
    /// Updates the status of a task asynchronously.
    /// </summary>
    /// <param name="id">The ID of the task.</param>
    /// <param name="userName">The username of the user.</param>
    /// <param name="taskItemEntity">The updated task item entity.</param>
    Task UpdateTaskStatusAsync(int id, string userName, TaskItemEntity taskItemEntity);

    /// <summary>
    /// Deletes a task asynchronously.
    /// </summary>
    /// <param name="id">The ID of the task.</param>
    /// <param name="todoListId">The ID of the to-do list.</param>
    /// <param name="userName">The username of the user.</param>
    Task DeleteAsync(int id, int todoListId, string userName);
}
