using TodoListApp.Models.Domains;
using TodoListApp.Models.DTOs;

namespace TodoListApp.WebApi.Interfaces;

public interface ICommentDatabaseService
{
    Task<Comment> GetCommentByIdAsync(int id, int taskId);

    Task<PagedModel<Comment>> GetPagedListOfCommentsAsync(int taskId, int page, int pageSize);

    Task<Comment> AddCommentToTaskAsync(Comment comment, string userName);

    Task UpdateCommentAsync(int id, int taskId, Comment comment, string userName);

    Task DeleteCommentAsync(int id, int taskId, string userName);
}
