using TodoListApp.Models.Domains;
using TodoListApp.Models.DTOs;

namespace TodoListApp.WebApp.Interfaces;

/// <summary>
/// Represents a web API service for managing tags.
/// </summary>
public interface ITagWebApiService
{
    /// <summary>
    /// Adds a tag to a task asynchronously.
    /// </summary>
    /// <param name="token">The authentication token.</param>
    /// <param name="todoListId">The ID of the to-do list.</param>
    /// <param name="taskId">The ID of the task.</param>
    /// <param name="tag">The tag to be added.</param>
    /// <returns>The added tag.</returns>
    Task<Tag> AddTagToTaskAsync(string token, int todoListId, int taskId, Tag tag);

    /// <summary>
    /// Deletes a tag asynchronously.
    /// </summary>
    /// <param name="token">The authentication token.</param>
    /// <param name="todoListId">The ID of the to-do list.</param>
    /// <param name="taskId">The ID of the task.</param>
    /// <param name="tagId">The ID of the tag to be deleted.</param>
    Task DeleteTagAsync(string token, int todoListId, int taskId, int tagId);

    /// <summary>
    /// Gets all tags asynchronously.
    /// </summary>
    /// <param name="token">The authentication token.</param>
    /// <param name="page">The page number.</param>
    /// <param name="pageSize">The number of items per page.</param>
    /// <returns>A paged collection of tags.</returns>
    Task<PagedModel<Tag>> GetAllTagsAsync(string token, int page, int pageSize);

    /// <summary>
    /// Gets a tag by ID asynchronously.
    /// </summary>
    /// <param name="token">The authentication token.</param>
    /// <param name="todoListId">The ID of the to-do list.</param>
    /// <param name="taskId">The ID of the task.</param>
    /// <param name="tagId">The ID of the tag.</param>
    /// <returns>The tag with the specified ID.</returns>
    Task<Tag> GetTagByIdAsync(string token, int todoListId, int taskId, int tagId);

    /// <summary>
    /// Gets tasks with a specific tag asynchronously.
    /// </summary>
    /// <param name="token">The authentication token.</param>
    /// <param name="tag">The tag to filter tasks by.</param>
    /// <param name="page">The page number.</param>
    /// <param name="pageSize">The number of items per page.</param>
    /// <returns>A paged collection of tasks with the specified tag.</returns>
    Task<PagedModel<TaskItem>> GetTasksWithTag(string token, string tag, int page, int pageSize);
}
