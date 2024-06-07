using TodoListApp.WebApi.Data.Entities;

namespace TodoListApp.WebApi.Abstractions;

public interface ITaskItemRepository
{
    Task<TaskItemEntity> GetByIdAsync(int id, int todoListId, string ownerId);

    Task<IEnumerable<TaskItemEntity>> GetAllAsync(int todoListId, string ownerId);

    Task<IEnumerable<TaskItemEntity>> GetAssignedToUserAsync(string userId, string? status, string? sort);

    Task<TaskItemEntity> CreateAsync(TaskItemEntity taskItemEntity);

    Task UpdateAsync(int id, int todoListId, TaskItemEntity taskItemEntity);

    Task UpdateTaskStatusAsync(int id, string userId, string status);

    Task DeleteAsync(int id, int todoListId, string ownerId);
}
