using Microsoft.EntityFrameworkCore;
using TodoListApp.Models.DTOs;
using TodoListApp.Models.Enums;
using TodoListApp.WebApi.Abstractions;
using TodoListApp.WebApi.Data;
using TodoListApp.WebApi.Data.Entities;

namespace TodoListApp.WebApi.Repositories;

public class TaskItemRepository : ITaskItemRepository
{
    private readonly TodoListDbContext context;

    public TaskItemRepository(TodoListDbContext context)
    {
        this.context = context;
    }

    public async Task<TaskItemEntity> CreateAsync(TaskItemEntity taskItemEntity)
    {
        ArgumentNullException.ThrowIfNull(taskItemEntity);

        _ = await this.context.TodoLists
            .Where(t => t.UserId == taskItemEntity.OwnerId)
            .FirstOrDefaultAsync(t => t.Id == taskItemEntity.TodoListId)
            ?? throw new KeyNotFoundException($"To-do list (id = {taskItemEntity.TodoListId}) not found.");

        var createdTask = await this.context.Tasks.AddAsync(taskItemEntity);
        _ = await this.context.SaveChangesAsync();

        return createdTask.Entity;
    }

    public async Task<PagedModel<TaskItemEntity>> GetListAsync(int todoListId, string ownerId, int page, int pageSize)
    {
        _ = await this.context.TodoLists
            .Where(t => t.UserId == ownerId)
            .FirstOrDefaultAsync(t => t.Id == todoListId)
            ?? throw new KeyNotFoundException($"To-do list (id = {todoListId}) not found.");

        var tasks = await this.context.Tasks
            .AsNoTracking()
            .Where(t => t.TodoListId == todoListId && t.OwnerId == ownerId)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var totalCount = await this.context.Tasks
           .Where(t => t.TodoListId == todoListId && t.OwnerId == ownerId)
           .CountAsync();

        var pagedModel = new PagedModel<TaskItemEntity>
        {
            Items = tasks,
            CurrentPage = page,
            ItemsPerPage = pageSize,
            TotalCount = totalCount,
        };

        return pagedModel;
    }

    public async Task<PagedModel<TaskItemEntity>> GetPagedListOfAssignedToUserAsync(string userName, int page, int pageSize, string? status, string? sort)
    {
        var tasks = this.context.Tasks
            .Where(t => t.Assignee == userName)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .AsNoTracking();

        var totalCount = await this.context.Tasks
           .Where(t => t.Assignee == userName)
           .CountAsync();

        if (!string.IsNullOrEmpty(status))
        {
            tasks = tasks.Where(t => t.Status == Enum.Parse<TaskItemStatus>(status, true));
        }

        if (!string.IsNullOrEmpty(sort))
        {
            if (sort.Equals("title", StringComparison.OrdinalIgnoreCase))
            {
                tasks = tasks.OrderBy(t => t.Title);
            }
            else if (sort.Equals("dueDate", StringComparison.OrdinalIgnoreCase))
            {
                tasks = tasks.OrderBy(t => t.DueDate == null).ThenBy(t => t.DueDate);
            }
        }

        var listOfTasks = await tasks.ToListAsync();

        var pagedModel = new PagedModel<TaskItemEntity>
        {
            Items = listOfTasks,
            CurrentPage = page,
            ItemsPerPage = pageSize,
            TotalCount = totalCount,
        };

        return pagedModel;
    }

    public async Task<TaskItemEntity> GetByIdAsync(int id, int todoListId, string ownerId)
    {
        _ = await this.context.TodoLists
            .Where(t => t.UserId == ownerId)
            .FirstOrDefaultAsync(t => t.Id == todoListId)
            ?? throw new KeyNotFoundException($"To-do list (id = {todoListId}) not found.");

        return await this.context.Tasks
            .Where(t => t.TodoListId == todoListId && t.OwnerId == ownerId)
            .FirstOrDefaultAsync(t => t.Id == id)
            ?? throw new KeyNotFoundException($"Task (id = {id}) not found.");
    }

    public async Task UpdateAsync(int id, int todoListId, TaskItemEntity taskItemEntity)
    {
        ArgumentNullException.ThrowIfNull(taskItemEntity);

        _ = await this.context.TodoLists
            .Where(t => t.UserId == taskItemEntity.OwnerId)
            .FirstOrDefaultAsync(t => t.Id == todoListId)
            ?? throw new KeyNotFoundException($"To-do list (id = {todoListId}) not found.");

        var existingTask = await this.context.Tasks
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id == id)
            ?? throw new KeyNotFoundException($"Task (id = {id}) not found.");

        taskItemEntity.TodoListId = todoListId;
        taskItemEntity.Id = existingTask.Id;

        if (string.IsNullOrEmpty(taskItemEntity.Assignee))
        {
            taskItemEntity.Assignee = taskItemEntity.OwnerId;
        }

        this.context.Entry(taskItemEntity).State = EntityState.Modified;

        _ = await this.context.SaveChangesAsync();
    }

    public async Task UpdateTaskStatusAsync(int id, string userId, string status)
    {
        var task = await this.context.Tasks
            .AsNoTracking()
            .Where(t => t.Assignee == userId)
            .FirstOrDefaultAsync(t => t.Id == id)
            ?? throw new KeyNotFoundException($"Task (id = {id}) not found.");

        task.Status = Enum.Parse<TaskItemStatus>(status, true);

        this.context.Entry(task).State = EntityState.Modified;

        _ = await this.context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id, int todoListId, string ownerId)
    {
        _ = await this.context.TodoLists
            .Where(t => t.UserId == ownerId)
            .FirstOrDefaultAsync(t => t.Id == todoListId)
            ?? throw new KeyNotFoundException($"To-do list (id = {todoListId}) not found.");

        var task = await this.context.Tasks
            .Where(t => t.TodoListId == todoListId && t.OwnerId == ownerId)
            .FirstOrDefaultAsync(t => t.Id == id)
            ?? throw new KeyNotFoundException($"Task (id = {id}) not found.");

        _ = this.context.Tasks.Remove(task);
        _ = await this.context.SaveChangesAsync();
    }
}
