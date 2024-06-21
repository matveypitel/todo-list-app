using TodoListApp.Models.Domains;
using TodoListApp.Models.DTOs;

namespace TodoListApp.WebApp.Interfaces;

public interface ICommentWebApiService
{
    Task<Comment> GetCommentByIdAsync(string token, int id, int todoListId, int taskId);

    Task<PagedModel<Comment>> GetPagedListOfCommentsAsync(string token, int taskId, int todoListId, int page, int pageSize);

    Task<Comment> AddCommentToTaskAsync(string token, int todoListId, int taskId, Comment comment);

    Task UpdateCommentAsync(string token, int id, int todoListId, int taskId, Comment comment);

    Task DeleteCommentAsync(string token, int id, int todoListId, int taskId);
}
