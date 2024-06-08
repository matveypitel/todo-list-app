using Microsoft.EntityFrameworkCore;
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

    public async Task<IEnumerable<TaskItemEntity>> GetAllAsync(int todoListId, string ownerId)
    {
        _ = await this.context.TodoLists
            .Where(t => t.UserId == ownerId)
            .FirstOrDefaultAsync(t => t.Id == todoListId)
            ?? throw new KeyNotFoundException($"To-do list (id = {todoListId}) not found.");

        return await this.context.Tasks
            .AsNoTracking()
            .Where(t => t.TodoListId == todoListId && t.OwnerId == ownerId)
            .ToListAsync();
    }

    public async Task<IEnumerable<TaskItemEntity>> GetAssignedToUserAsync(string userId, string? status, string? sort)
    {
        var tasks = this.context.Tasks
        .AsNoTracking()
        .Where(t => t.Assignee == userId);

        if (!string.IsNullOrEmpty(status))
        {
            tasks = tasks.Where(t => t.Status == Enum.Parse<TaskItemStatus>(status, true));
        }

        if (!string.IsNullOrEmpty(sort))
        {
            if (sort == "title")
            {
                tasks = tasks.OrderBy(t => t.Title);
            }
            else if (sort == "dueDate")
            {
                tasks = tasks.OrderBy(t => t.DueDate);
            }
        }

        return await tasks.ToListAsync();
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
            taskItemEntity.Assignee = existingTask.Assignee;
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
