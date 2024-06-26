using TodoListApp.Models.DTOs;
using TodoListApp.WebApi.Data.Entities;

namespace TodoListApp.WebApi.Interfaces;

/// <summary>
/// Represents a repository for managing tags.
/// </summary>
public interface ITagRepository
{
    /// <summary>
    /// Retrieves a tag by its ID.
    /// </summary>
    /// <param name="id">The ID of the tag.</param>
    /// <param name="taskId">The ID of the task associated with the tag.</param>
    /// <param name="userName">The username of the user.</param>
    /// <returns>The tag entity.</returns>
    Task<TagEntity> GetByIdAsync(int id, int taskId, string userName);

    /// <summary>
    /// Retrieves a paged list of all tags.
    /// </summary>
    /// <param name="userName">The username of the user.</param>
    /// <param name="page">The page number.</param>
    /// <param name="pageSize">The number of items per page.</param>
    /// <returns>The paged list of tag entities.</returns>
    Task<PagedModel<TagEntity>> GetPagedListOfAllAsync(string userName, int page, int pageSize);

    /// <summary>
    /// Adds a tag to a task.
    /// </summary>
    /// <param name="taskId">The ID of the task.</param>
    /// <param name="tagEntity">The tag entity to be added.</param>
    /// <param name="userName">The username of the user.</param>
    /// <returns>The added tag entity.</returns>
    Task<TagEntity> AddToTaskAsync(int taskId, TagEntity tagEntity, string userName);

    /// <summary>
    /// Deletes a tag by its ID.
    /// </summary>
    /// <param name="id">The ID of the tag.</param>
    /// <param name="taskId">The ID of the task associated with the tag.</param>
    /// <param name="userName">The username of the user.</param>
    Task DeleteAsync(int id, int taskId, string userName);
}
