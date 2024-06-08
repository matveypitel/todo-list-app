using TodoListApp.Models.Domains;

namespace TodoListApp.WebApi.Abstractions;

public interface ITaskItemDatabaseService
{
    Task<TaskItem> GetTaskByIdAsync(int id, int todoListId, string ownerId);

    Task<IEnumerable<TaskItem>> GetListOfTasksAsync(int todoListId, string ownerId);

    Task<IEnumerable<TaskItem>> GetAssignedTaskToUserAsync(string userId, string? status, string? sort);

    Task<TaskItem> CreateTaskAsync(TaskItem taskItem);

    Task UpdateTaskAsync(int id, int todoListId, TaskItem taskItem);

    Task UpdateTaskStatusAsync(int id, string userId, string status);

    Task DeleteTaskAsync(int id, int todoListId, string ownerId);
}
