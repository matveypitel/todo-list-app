using TodoListApp.Models.DTOs;
using TodoListApp.WebApi.Data.Entities;

namespace TodoListApp.WebApi.Interfaces;

public interface ITagRepository
{
    Task<TagEntity> GetByIdAsync(int id, int taskId, string userName);

    Task<PagedModel<TagEntity>> GetPagedListOfAllAsync(string userName, int page, int pageSize);

    Task<TagEntity> AddToTaskAsync(int taskId, TagEntity tagEntity, string userName);

    Task DeleteAsync(int id, int taskId, string userName);
}
