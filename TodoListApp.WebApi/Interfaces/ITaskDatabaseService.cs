using TodoListApp.Models.Domains;
using TodoListApp.Models.DTOs;
using TodoListApp.Models.Enums;

namespace TodoListApp.WebApi.Interfaces;

/// <summary>
/// Represents a service for interacting with the task database.
/// </summary>
public interface ITaskDatabaseService
{
    /// <summary>
    /// Gets the user's role in a specific to-do list.
    /// </summary>
    /// <param name="todoListId">The ID of the to-do list.</param>
    /// <param name="userName">The username of the user.</param>
    /// <returns>The user's role in the to-do list.</returns>
    Task<TodoListRole> GetUserRoleInTodoListAsync(int todoListId, string userName);

    /// <summary>
    /// Gets a task by its ID.
    /// </summary>
    /// <param name="id">The ID of the task.</param>
    /// <param name="todoListId">The ID of the to-do list.</param>
    /// <param name="userName">The username of the user.</param>
    /// <returns>The task with the specified ID.</returns>
    Task<TaskItem> GetTaskByIdAsync(int id, int todoListId, string userName);

    /// <summary>
    /// Gets an assigned task by its ID.
    /// </summary>
    /// <param name="id">The ID of the task.</param>
    /// <param name="userName">The username of the user.</param>
    /// <returns>The assigned task with the specified ID.</returns>
    Task<TaskItem> GetAssignedTaskByIdAsync(int id, string userName);

    /// <summary>
    /// Gets a paged list of tasks in a specific to-do list.
    /// </summary>
    /// <param name="todoListId">The ID of the to-do list.</param>
    /// <param name="userName">The username of the user.</param>
    /// <param name="page">The page number.</param>
    /// <param name="pageSize">The number of tasks per page.</param>
    /// <returns>A paged list of tasks.</returns>
    Task<PagedModel<TaskItem>> GetPagedListOfTasksAsync(int todoListId, string userName, int page, int pageSize);

    /// <summary>
    /// Gets a paged list of tasks with a specific tag.
    /// </summary>
    /// <param name="userName">The username of the user.</param>
    /// <param name="tagLabel">The label of the tag.</param>
    /// <param name="page">The page number.</param>
    /// <param name="pageSize">The number of tasks per page.</param>
    /// <returns>A paged list of tasks with the specified tag.</returns>
    Task<PagedModel<TaskItem>> GetPagedListOfTasksWithTagAsync(string userName, string tagLabel, int page, int pageSize);

    /// <summary>
    /// Gets a paged list of tasks assigned to a specific user.
    /// </summary>
    /// <param name="userName">The username of the user.</param>
    /// <param name="page">The page number.</param>
    /// <param name="pageSize">The number of tasks per page.</param>
    /// <param name="status">The status of the tasks (optional).</param>
    /// <param name="sort">The sort order of the tasks (optional).</param>
    /// <returns>A paged list of tasks assigned to the user.</returns>
    Task<PagedModel<TaskItem>> GetPagedListOfAssignedTasksToUserAsync(string userName, int page, int pageSize, string? status, string? sort);

    /// <summary>
    /// Gets a paged list of task search results.
    /// </summary>
    /// <param name="userName">The username of the user.</param>
    /// <param name="title">The title of the task (optional).</param>
    /// <param name="creationDate">The creation date of the task (optional).</param>
    /// <param name="dueDate">The due date of the task (optional).</param>
    /// <param name="page">The page number.</param>
    /// <param name="pageSize">The number of tasks per page.</param>
    /// <returns>A paged list of task search results.</returns>
    Task<PagedModel<TaskItem>> GetPagedListOfTasksSearchResultsAsync(string userName, string? title, string? creationDate, string? dueDate, int page, int pageSize);

    /// <summary>
    /// Creates a new task.
    /// </summary>
    /// <param name="taskItem">The task item to create.</param>
    /// <param name="userName">The username of the user.</param>
    /// <returns>The created task.</returns>
    Task<TaskItem> CreateTaskAsync(TaskItem taskItem, string userName);

    /// <summary>
    /// Updates a task.
    /// </summary>
    /// <param name="id">The ID of the task.</param>
    /// <param name="todoListId">The ID of the to-do list.</param>
    /// <param name="taskItem">The updated task item.</param>
    /// <param name="userName">The username of the user.</param>
    Task UpdateTaskAsync(int id, int todoListId, TaskItem taskItem, string userName);

    /// <summary>
    /// Updates the status of a task.
    /// </summary>
    /// <param name="id">The ID of the task.</param>
    /// <param name="userName">The username of the user.</param>
    /// <param name="taskItem">The updated task item.</param>
    Task UpdateTaskStatusAsync(int id, string userName, TaskItem taskItem);

    /// <summary>
    /// Deletes a task.
    /// </summary>
    /// <param name="id">The ID of the task.</param>
    /// <param name="todoListId">The ID of the to-do list.</param>
    /// <param name="userName">The username of the user.</param>
    Task DeleteTaskAsync(int id, int todoListId, string userName);
}
