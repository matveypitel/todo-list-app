using TodoListApp.Models.Domains;
using TodoListApp.Models.DTOs;

namespace TodoListApp.WebApi.Abstractions;

public interface ITaskItemDatabaseService
{
    Task<TaskItem> GetTaskByIdAsync(int id, int todoListId, string ownerId);

    Task<IEnumerable<TaskItem>> GetListOfTasksAsync(int todoListId, string ownerId);

    Task<PagedModel<TaskItem>> GetPagedListOfAssignedTaskToUserAsync(string userId, int page, int pageSize, string? status, string? sort);

    Task<TaskItem> CreateTaskAsync(TaskItem taskItem);

    Task UpdateTaskAsync(int id, int todoListId, TaskItem taskItem);

    Task UpdateTaskStatusAsync(int id, string userId, string status);

    Task DeleteTaskAsync(int id, int todoListId, string ownerId);
}
