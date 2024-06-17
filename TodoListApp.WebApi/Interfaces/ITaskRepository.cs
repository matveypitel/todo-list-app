using TodoListApp.Models.DTOs;
using TodoListApp.WebApi.Data.Entities;

namespace TodoListApp.WebApi.Interfaces;

public interface ITaskRepository
{
    Task<TaskItemEntity> GetByIdAsync(int id, int todoListId, string userName);

    Task<TaskItemEntity> GetAssignedByIdAsync(int id, string userName);

    Task<PagedModel<TaskItemEntity>> GetPagedListAsync(int todoListId, string userName, int page, int pageSize);

    Task<PagedModel<TaskItemEntity>> GetPagedListWithTagAsync(string userName, string tagLabel, int page, int pageSize);

    Task<PagedModel<TaskItemEntity>> GetPagedListOfAssignedToUserAsync(string userName, int page, int pageSize, string? status, string? sort);

    Task<PagedModel<TaskItemEntity>> GetPagedListOfSearchResultsAsync(string userName, string? title, DateTime? creationDate, DateTime? dueDate, int page, int pageSize);

    Task<TaskItemEntity> CreateAsync(TaskItemEntity taskItemEntity, string userName);

    Task UpdateAsync(int id, int todoListId, TaskItemEntity taskItemEntity, string userName);

    Task UpdateTaskStatusAsync(int id, string userName, TaskItemEntity taskItemEntity);

    Task DeleteAsync(int id, int todoListId, string userName);
}
