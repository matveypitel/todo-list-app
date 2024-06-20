using TodoListApp.Models.DTOs;
using TodoListApp.WebApi.Data.Entities;

namespace TodoListApp.WebApi.Interfaces;

public interface ICommentRepository
{
    Task<CommentEntity> GetByIdAsync(int id, int taskId);

    Task<PagedModel<CommentEntity>> GetPagedListOfAllAsync(int taskId, int page, int pageSize);

    Task<CommentEntity> AddToTaskAsync(CommentEntity commentEntity, string userName);

    Task UpdateAsync(int id, int taskId, CommentEntity commentEntity, string userName);

    Task DeleteAsync(int id, int taskId, string userName);
}
