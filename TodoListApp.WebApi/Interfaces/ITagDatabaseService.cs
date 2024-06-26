using TodoListApp.Models.Domains;
using TodoListApp.Models.DTOs;

namespace TodoListApp.WebApi.Interfaces;

/// <summary>
/// Represents a service for managing tags in the database.
/// </summary>
public interface ITagDatabaseService
{
    /// <summary>
    /// Adds a tag to a task asynchronously.
    /// </summary>
    /// <param name="taskId">The ID of the task.</param>
    /// <param name="tag">The tag to be added.</param>
    /// <param name="userName">The username of the user.</param>
    /// <returns>The added tag.</returns>
    Task<Tag> AddTagToTaskAsync(int taskId, Tag tag, string userName);

    /// <summary>
    /// Gets a paged list of all tags asynchronously.
    /// </summary>
    /// <param name="userName">The username of the user.</param>
    /// <param name="page">The page number.</param>
    /// <param name="pageSize">The number of items per page.</param>
    /// <returns>A paged list of tags.</returns>
    Task<PagedModel<Tag>> GetPagedListOfAllAsync(string userName, int page, int pageSize);

    /// <summary>
    /// Gets a tag by ID asynchronously.
    /// </summary>
    /// <param name="id">The ID of the tag.</param>
    /// <param name="taskId">The ID of the task.</param>
    /// <param name="userName">The username of the user.</param>
    /// <returns>The tag with the specified ID.</returns>
    Task<Tag> GetTagByIdAsync(int id, int taskId, string userName);

    /// <summary>
    /// Deletes a tag asynchronously.
    /// </summary>
    /// <param name="id">The ID of the tag.</param>
    /// <param name="taskId">The ID of the task.</param>
    /// <param name="userName">The username of the user.</param>
    Task DeleteTagAsync(int id, int taskId, string userName);
}
