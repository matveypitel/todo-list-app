using TodoListApp.Models.Domains;
using TodoListApp.Models.DTOs;
using TodoListApp.WebApi.Data.Entities;

namespace TodoListApp.WebApi.Interfaces;

public interface ITaskDatabaseService
{
    Task<TaskItem> GetTaskByIdAsync(int id, int todoListId, string ownerName);

    Task<TaskItem> GetAssignedTaskByIdAsync(int id, string userName);

    Task<PagedModel<TaskItem>> GetPagedListOfTasksAsync(int todoListId, string ownerName, int page, int pageSize);

    Task<PagedModel<TaskItem>> GetPagedListOfTasksWithTagAsync(string userName, string tagLabel, int page, int pageSize);

    Task<PagedModel<TaskItem>> GetPagedListOfAssignedTasksToUserAsync(string userName, int page, int pageSize, string? status, string? sort);

    Task<PagedModel<TaskItem>> GetPagedListOfTasksSearchResultsAsync(string userName, string? title, DateTime? creationDate, DateTime? dueDate, int page, int pageSize);

    Task<TaskItem> CreateTaskAsync(TaskItem taskItem);

    Task UpdateTaskAsync(int id, int todoListId, TaskItem taskItem);

    Task UpdateTaskStatusAsync(int id, string userName, TaskItem taskItem);

    Task DeleteTaskAsync(int id, int todoListId, string ownerName);
}
