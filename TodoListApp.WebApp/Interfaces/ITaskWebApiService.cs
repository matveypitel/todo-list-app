using Microsoft.AspNetCore.Mvc;
using TodoListApp.Models.Domains;
using TodoListApp.Models.DTOs;
using TodoListApp.Models.Enums;

namespace TodoListApp.WebApp.Interfaces;

public interface ITaskWebApiService
{
    Task<TodoListRole> GetUserRoleInTodoListAsync(string token, int todoListId);

    Task<PagedModel<TaskItem>> GetPagedTasksAsync(string token, int todoListId, int page, int pageSize);

    Task<PagedModel<TaskItem>> GetPagedSearchedTaskAsync(string token, int page, int pageSize, string? title, string? creationDate, string? dueDate);

    Task<TaskItem> GetTaskByIdAsync(string token, int id, int todoListId);

    Task<TaskItem> CreateTaskAsync(string token, int todoListId, TaskItem task);

    Task UpdateTaskAsync(string token, int id, int todoListId, TaskItem task);

    Task DeleteTaskAsync(string token, int id, int todoListId);

    Task<PagedModel<TaskItem>> GetAssignedTasksToUserAsync(string token, int page, int pageSize, string? status, string? sort);

    Task<TaskItem> GetAssignedTaskByIdAsync(string token, int id);

    Task UpdateTaskStatusAsync(string token, int id, TaskItem task);
}
