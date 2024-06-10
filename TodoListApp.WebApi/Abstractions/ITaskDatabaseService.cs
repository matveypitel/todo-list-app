using TodoListApp.Models.Domains;
using TodoListApp.Models.DTOs;

namespace TodoListApp.WebApi.Abstractions;

public interface ITaskDatabaseService
{
    Task<TaskItem> GetTaskByIdAsync(int id, int todoListId, string ownerId);

    Task<PagedModel<TaskItem>> GetListOfTasksAsync(int todoListId, string ownerId, int page, int pageSize);

    Task<PagedModel<TaskItem>> GetPagedListOfAssignedTaskToUserAsync(string userName, int page, int pageSize, string? status, string? sort);

    Task<TaskItem> CreateTaskAsync(TaskItem taskItem);

    Task UpdateTaskAsync(int id, int todoListId, TaskItem taskItem);

    Task UpdateTaskStatusAsync(int id, string userId, string status);

    Task DeleteTaskAsync(int id, int todoListId, string ownerId);
}
