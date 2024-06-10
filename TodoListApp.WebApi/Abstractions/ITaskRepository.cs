using TodoListApp.Models.DTOs;
using TodoListApp.WebApi.Data.Entities;

namespace TodoListApp.WebApi.Abstractions;

public interface ITaskRepository
{
    Task<TaskItemEntity> GetByIdAsync(int id, int todoListId, string ownerName);

    Task<PagedModel<TaskItemEntity>> GetListAsync(int todoListId, string ownerName, int page, int pageSize);

    Task<PagedModel<TaskItemEntity>> GetPagedListOfAssignedToUserAsync(string userName, int page, int pageSize, string? status, string? sort);

    Task<TaskItemEntity> CreateAsync(TaskItemEntity taskItemEntity);

    Task UpdateAsync(int id, int todoListId, TaskItemEntity taskItemEntity);

    Task UpdateTaskStatusAsync(int id, string userName, string status);

    Task DeleteAsync(int id, int todoListId, string ownerName);
}
