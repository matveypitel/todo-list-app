using TodoListApp.Models.Domains;

namespace TodoListApp.WebApi.Abstractions;

public interface ITaskItemDatabaseService
{
    Task<TaskItem> GetByIdAsync(int id, int todoListId, string ownerId);

    Task<IEnumerable<TaskItem>> GetAllAsync(int todoListId, string ownerId);

    Task<IEnumerable<TaskItem>> GetAssignedToUserAsync(string userId, string? status, string? sort);

    Task<TaskItem> CreateAsync(TaskItem taskItem);

    Task UpdateAsync(int id, int todoListId, TaskItem taskItem);

    Task UpdateTaskStatusAsync(int id, string userId, string status);

    Task DeleteAsync(int id, int todoListId, string ownerId);
}
