using AutoMapper;
using TodoListApp.Models.Domains;
using TodoListApp.Models.DTOs;
using TodoListApp.Models.Enums;
using TodoListApp.WebApi.Data.Entities;
using TodoListApp.WebApi.Interfaces;

namespace TodoListApp.WebApi.Services;

/// <summary>
/// Represents a service for interacting with the task database.
/// </summary>
public class TaskDatabaseService : ITaskDatabaseService
{
    private readonly ITaskRepository repository;
    private readonly IMapper mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="TaskDatabaseService"/> class.
    /// </summary>
    /// <param name="context">The TodoListDbContext.</param>
    public TaskDatabaseService(ITaskRepository repository, IMapper mapper)
    {
        this.repository = repository;
        this.mapper = mapper;
    }

    /// <inheritdoc/>
    public async Task<TodoListRole> GetUserRoleInTodoListAsync(int todoListId, string userName)
    {
        return await this.repository.GetUserRoleInTodoListAsync(todoListId, userName);
    }

    /// <inheritdoc/>
    public async Task<TaskItem> CreateTaskAsync(TaskItem taskItem, string userName)
    {
        var taskEntity = await this.repository.CreateAsync(this.mapper.Map<TaskItemEntity>(taskItem), userName);
        return this.mapper.Map<TaskItem>(taskEntity);
    }

    /// <inheritdoc/>
    public async Task<PagedModel<TaskItem>> GetPagedListOfTasksAsync(int todoListId, string userName, int page, int pageSize)
    {
        var tasks = await this.repository.GetPagedListAsync(todoListId, userName, page, pageSize);
        return this.mapper.Map<PagedModel<TaskItem>>(tasks);
    }

    /// <inheritdoc/>
    public async Task<PagedModel<TaskItem>> GetPagedListOfTasksWithTagAsync(string userName, string tagLabel, int page, int pageSize)
    {
        var tasks = await this.repository.GetPagedListWithTagAsync(userName, tagLabel, page, pageSize);
        return this.mapper.Map<PagedModel<TaskItem>>(tasks);
    }

    /// <inheritdoc/>
    public async Task<PagedModel<TaskItem>> GetPagedListOfAssignedTasksToUserAsync(string userName, int page, int pageSize, string? status, string? sort)
    {
        var assignedTasks = await this.repository.GetPagedListOfAssignedToUserAsync(userName, page, pageSize, status, sort);
        return this.mapper.Map<PagedModel<TaskItem>>(assignedTasks);
    }

    /// <inheritdoc/>
    public async Task<PagedModel<TaskItem>> GetPagedListOfTasksSearchResultsAsync(string userName, string? title, string? creationDate, string? dueDate, int page, int pageSize)
    {
        var searchedTasks = await this.repository.GetPagedListOfSearchResultsAsync(userName, title, creationDate, dueDate, page, pageSize);
        return this.mapper.Map<PagedModel<TaskItem>>(searchedTasks);
    }

    /// <inheritdoc/>
    public async Task<TaskItem> GetTaskByIdAsync(int id, int todoListId, string userName)
    {
        var task = await this.repository.GetByIdAsync(id, todoListId, userName);
        return this.mapper.Map<TaskItem>(task);
    }

    /// <inheritdoc/>
    public async Task UpdateTaskAsync(int id, int todoListId, TaskItem taskItem, string userName)
    {
        var taskEntity = this.mapper.Map<TaskItemEntity>(taskItem);
        await this.repository.UpdateAsync(id, todoListId, taskEntity, userName);
    }

    /// <inheritdoc/>
    public async Task UpdateTaskStatusAsync(int id, string userName, TaskItem taskItem)
    {
        var taskEntity = this.mapper.Map<TaskItemEntity>(taskItem);
        await this.repository.UpdateTaskStatusAsync(id, userName, taskEntity);
    }

    /// <inheritdoc/>
    public async Task DeleteTaskAsync(int id, int todoListId, string userName)
    {
        await this.repository.DeleteAsync(id, todoListId, userName);
    }

    /// <inheritdoc/>
    public async Task<TaskItem> GetAssignedTaskByIdAsync(int id, string userName)
    {
        var task = await this.repository.GetAssignedByIdAsync(id, userName);
        return this.mapper.Map<TaskItem>(task);
    }
}
