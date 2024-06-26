using TodoListApp.Models.Domains;
using TodoListApp.Models.DTOs;

namespace TodoListApp.WebApi.Interfaces;

/// <summary>
/// Represents a database service for managing comments.
/// </summary>
public interface ICommentDatabaseService
{
    /// <summary>
    /// Retrieves a comment by its ID and task ID.
    /// </summary>
    /// <param name="id">The ID of the comment.</param>
    /// <param name="taskId">The ID of the task.</param>
    /// <returns>The comment with the specified ID and task ID.</returns>
    Task<Comment> GetCommentByIdAsync(int id, int taskId);

    /// <summary>
    /// Retrieves a paged list of comments for a specific task.
    /// </summary>
    /// <param name="taskId">The ID of the task.</param>
    /// <param name="page">The page number.</param>
    /// <param name="pageSize">The number of comments per page.</param>
    /// <returns>A paged list of comments for the specified task.</returns>
    Task<PagedModel<Comment>> GetPagedListOfCommentsAsync(int taskId, int page, int pageSize);

    /// <summary>
    /// Adds a comment to a task.
    /// </summary>
    /// <param name="comment">The comment to add.</param>
    /// <param name="userName">The username of the user adding the comment.</param>
    /// <returns>The added comment.</returns>
    Task<Comment> AddCommentToTaskAsync(Comment comment, string userName);

    /// <summary>
    /// Updates a comment.
    /// </summary>
    /// <param name="id">The ID of the comment.</param>
    /// <param name="taskId">The ID of the task.</param>
    /// <param name="comment">The updated comment.</param>
    /// <param name="userName">The username of the user updating the comment.</param>
    Task UpdateCommentAsync(int id, int taskId, Comment comment, string userName);

    /// <summary>
    /// Deletes a comment.
    /// </summary>
    /// <param name="id">The ID of the comment.</param>
    /// <param name="taskId">The ID of the task.</param>
    /// <param name="userName">The username of the user deleting the comment.</param>
    Task DeleteCommentAsync(int id, int taskId, string userName);
}
