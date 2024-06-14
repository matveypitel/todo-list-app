using TodoListApp.Models.DTOs;
using TodoListApp.WebApi.Data.Entities;

namespace TodoListApp.WebApi.Interfaces;

public interface ITagRepository
{
    Task<TagEntity> GetByIdAsync(int id, int taskId);

    Task<PagedModel<TagEntity>> GetPagedListOfAllAsync(string tasksOwnerName, int page, int pageSize);

    Task<TagEntity> AddToTaskAsync(int taskId, TagEntity tagEntity);

    Task UpdateAsync(int id, int taskId, TagEntity tagEntity);

    Task DeleteAsync(int id, int taskId);

    Task TagTaskExistsAsync(int taskId);
}
