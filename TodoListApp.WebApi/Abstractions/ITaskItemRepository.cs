using TodoListApp.Models.DTOs;
using TodoListApp.WebApi.Data.Entities;

namespace TodoListApp.WebApi.Abstractions;

public interface ITaskItemRepository
{
    Task<TaskItemEntity> GetByIdAsync(int id, int todoListId, string ownerId);

    Task<IEnumerable<TaskItemEntity>> GetListAsync(int todoListId, string ownerId);

    Task<PagedModel<TaskItemEntity>> GetPagedListOfAssignedToUserAsync(string userId, int page, int pageSize, string? status, string? sort);

    Task<TaskItemEntity> CreateAsync(TaskItemEntity taskItemEntity);

    Task UpdateAsync(int id, int todoListId, TaskItemEntity taskItemEntity);

    Task UpdateTaskStatusAsync(int id, string userId, string status);

    Task DeleteAsync(int id, int todoListId, string ownerId);
}
