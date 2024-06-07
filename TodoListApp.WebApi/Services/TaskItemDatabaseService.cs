using AutoMapper;
using TodoListApp.Models.Domains;
using TodoListApp.WebApi.Abstractions;
using TodoListApp.WebApi.Data.Entities;

namespace TodoListApp.WebApi.Services;

public class TaskItemDatabaseService : ITaskItemDatabaseService
{
    private readonly ITaskItemRepository repository;
    private readonly IMapper mapper;

    public TaskItemDatabaseService(ITaskItemRepository repository, IMapper mapper)
    {
        this.repository = repository;
        this.mapper = mapper;
    }

    public async Task<TaskItem> CreateAsync(TaskItem taskItem)
    {
        var taskEntity = await this.repository.CreateAsync(this.mapper.Map<TaskItemEntity>(taskItem));
        return this.mapper.Map<TaskItem>(taskEntity);
    }

    public async Task<IEnumerable<TaskItem>> GetAllAsync(int todoListId, string ownerId)
    {
        var tasks = await this.repository.GetAllAsync(todoListId, ownerId);
        return this.mapper.Map<IEnumerable<TaskItem>>(tasks);
    }

    public async Task<IEnumerable<TaskItem>> GetAssignedToUserAsync(string userId, string? status, string? sort)
    {
        var assignedTasks = await this.repository.GetAssignedToUserAsync(userId, status, sort);
        return this.mapper.Map<IEnumerable<TaskItem>>(assignedTasks);
    }

    public async Task<TaskItem> GetByIdAsync(int id, int todoListId, string ownerId)
    {
        var task = await this.repository.GetByIdAsync(id, todoListId, ownerId);
        return this.mapper.Map<TaskItem>(task);
    }

    public async Task UpdateAsync(int id, int todoListId, TaskItem taskItem)
    {
        var taskEntity = this.mapper.Map<TaskItemEntity>(taskItem);
        await this.repository.UpdateAsync(id, todoListId, taskEntity);
    }

    public async Task UpdateTaskStatusAsync(int id, string userId, string status)
    {
        await this.repository.UpdateTaskStatusAsync(id, userId, status);
    }

    public async Task DeleteAsync(int id, int todoListId, string ownerId)
    {
        await this.repository.DeleteAsync(id, todoListId, ownerId);
    }
}
