using TodoListApp.Models.DTOs;
using TodoListApp.WebApi.Data.Entities;

namespace TodoListApp.WebApi.Interfaces;

/// <summary>
/// Represents a repository for managing comments.
/// </summary>
public interface ICommentRepository
{
    /// <summary>
    /// Retrieves a comment by its ID and task ID.
    /// </summary>
    /// <param name="id">The ID of the comment.</param>
    /// <param name="taskId">The ID of the task.</param>
    /// <returns>The comment entity.</returns>
    Task<CommentEntity> GetByIdAsync(int id, int taskId);

    /// <summary>
    /// Retrieves a paged list of all comments for a specific task.
    /// </summary>
    /// <param name="taskId">The ID of the task.</param>
    /// <param name="page">The page number.</param>
    /// <param name="pageSize">The number of comments per page.</param>
    /// <returns>The paged list of comment entities.</returns>
    Task<PagedModel<CommentEntity>> GetPagedListOfAllAsync(int taskId, int page, int pageSize);

    /// <summary>
    /// Adds a comment to a task.
    /// </summary>
    /// <param name="commentEntity">The comment entity to add.</param>
    /// <param name="userName">The username of the user adding the comment.</param>
    /// <returns>The added comment entity.</returns>
    Task<CommentEntity> AddToTaskAsync(CommentEntity commentEntity, string userName);

    /// <summary>
    /// Updates a comment by its ID and task ID.
    /// </summary>
    /// <param name="id">The ID of the comment.</param>
    /// <param name="taskId">The ID of the task.</param>
    /// <param name="commentEntity">The updated comment entity.</param>
    /// <param name="userName">The username of the user updating the comment.</param>
    Task UpdateAsync(int id, int taskId, CommentEntity commentEntity, string userName);

    /// <summary>
    /// Deletes a comment by its ID and task ID.
    /// </summary>
    /// <param name="id">The ID of the comment.</param>
    /// <param name="taskId">The ID of the task.</param>
    /// <param name="userName">The username of the user deleting the comment.</param>
    Task DeleteAsync(int id, int taskId, string userName);
}
