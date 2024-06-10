using AutoMapper;
using TodoListApp.Models.Domains;
using TodoListApp.Models.DTOs;
using TodoListApp.WebApi.Abstractions;
using TodoListApp.WebApi.Data.Entities;

namespace TodoListApp.WebApi.Services;

public class TaskDatabaseService : ITaskDatabaseService
{
    private readonly ITaskRepository repository;
    private readonly IMapper mapper;

    public TaskDatabaseService(ITaskRepository repository, IMapper mapper)
    {
        this.repository = repository;
        this.mapper = mapper;
    }

    public async Task<TaskItem> CreateTaskAsync(TaskItem taskItem)
    {
        var taskEntity = await this.repository.CreateAsync(this.mapper.Map<TaskItemEntity>(taskItem));
        return this.mapper.Map<TaskItem>(taskEntity);
    }

    public async Task<PagedModel<TaskItem>> GetListOfTasksAsync(int todoListId, string ownerName, int page, int pageSize)
    {
        var tasks = await this.repository.GetListAsync(todoListId, ownerName, page, pageSize);
        return this.mapper.Map<PagedModel<TaskItem>>(tasks);
    }

    public async Task<PagedModel<TaskItem>> GetPagedListOfAssignedTaskToUserAsync(string userName, int page, int pageSize, string? status, string? sort)
    {
        var assignedTasks = await this.repository.GetPagedListOfAssignedToUserAsync(userName, page, pageSize, status, sort);
        return this.mapper.Map<PagedModel<TaskItem>>(assignedTasks);
    }

    public async Task<TaskItem> GetTaskByIdAsync(int id, int todoListId, string ownerName)
    {
        var task = await this.repository.GetByIdAsync(id, todoListId, ownerName);
        return this.mapper.Map<TaskItem>(task);
    }

    public async Task UpdateTaskAsync(int id, int todoListId, TaskItem taskItem)
    {
        var taskEntity = this.mapper.Map<TaskItemEntity>(taskItem);
        await this.repository.UpdateAsync(id, todoListId, taskEntity);
    }

    public async Task UpdateTaskStatusAsync(int id, string userName, string status)
    {
        await this.repository.UpdateTaskStatusAsync(id, userName, status);
    }

    public async Task DeleteTaskAsync(int id, int todoListId, string ownerName)
    {
        await this.repository.DeleteAsync(id, todoListId, ownerName);
    }
}
