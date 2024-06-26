using TodoListApp.Models.Domains;
using TodoListApp.Models.DTOs;

namespace TodoListApp.WebApp.Interfaces;

/// <summary>
/// Represents a web API service for managing comments.
/// </summary>
public interface ICommentWebApiService
{
    /// <summary>
    /// Retrieves a comment by its ID.
    /// </summary>
    /// <param name="token">The authentication token.</param>
    /// <param name="id">The ID of the comment.</param>
    /// <param name="todoListId">The ID of the to-do list.</param>
    /// <param name="taskId">The ID of the task.</param>
    /// <returns>The comment with the specified ID.</returns>
    Task<Comment> GetCommentByIdAsync(string token, int id, int todoListId, int taskId);

    /// <summary>
    /// Retrieves a paged list of comments for a task.
    /// </summary>
    /// <param name="token">The authentication token.</param>
    /// <param name="taskId">The ID of the task.</param>
    /// <param name="todoListId">The ID of the to-do list.</param>
    /// <param name="page">The page number.</param>
    /// <param name="pageSize">The number of comments per page.</param>
    /// <returns>A paged list of comments for the specified task.</returns>
    Task<PagedModel<Comment>> GetPagedListOfCommentsAsync(string token, int taskId, int todoListId, int page, int pageSize);

    /// <summary>
    /// Adds a comment to a task.
    /// </summary>
    /// <param name="token">The authentication token.</param>
    /// <param name="todoListId">The ID of the to-do list.</param>
    /// <param name="taskId">The ID of the task.</param>
    /// <param name="comment">The comment to add.</param>
    /// <returns>The added comment.</returns>
    Task<Comment> AddCommentToTaskAsync(string token, int todoListId, int taskId, Comment comment);

    /// <summary>
    /// Updates a comment.
    /// </summary>
    /// <param name="token">The authentication token.</param>
    /// <param name="id">The ID of the comment.</param>
    /// <param name="todoListId">The ID of the to-do list.</param>
    /// <param name="taskId">The ID of the task.</param>
    /// <param name="comment">The updated comment.</param>
    Task UpdateCommentAsync(string token, int id, int todoListId, int taskId, Comment comment);

    /// <summary>
    /// Deletes a comment.
    /// </summary>
    /// <param name="token">The authentication token.</param>
    /// <param name="id">The ID of the comment.</param>
    /// <param name="todoListId">The ID of the to-do list.</param>
    /// <param name="taskId">The ID of the task.</param>
    Task DeleteCommentAsync(string token, int id, int todoListId, int taskId);
}
