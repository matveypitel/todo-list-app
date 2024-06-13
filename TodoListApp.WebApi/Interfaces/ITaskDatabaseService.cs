using TodoListApp.Models.Domains;
using TodoListApp.Models.DTOs;

namespace TodoListApp.WebApi.Interfaces;

public interface ITaskDatabaseService
{
    Task<TaskItem> GetTaskByIdAsync(int id, int todoListId, string ownerName);

    Task<TaskItem> GetAssignedTaskByIdAsync(int id, string userName);

    Task<PagedModel<TaskItem>> GetListOfTasksAsync(int todoListId, string ownerName, int page, int pageSize);

    Task<PagedModel<TaskItem>> GetPagedListOfAssignedTaskToUserAsync(string userName, int page, int pageSize, string? status, string? sort);

    Task<TaskItem> CreateTaskAsync(TaskItem taskItem);

    Task UpdateTaskAsync(int id, int todoListId, TaskItem taskItem);

    Task UpdateTaskStatusAsync(int id, string userName, TaskItem taskItem);

    Task DeleteTaskAsync(int id, int todoListId, string ownerName);
}
