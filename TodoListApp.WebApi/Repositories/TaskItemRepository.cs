using Microsoft.EntityFrameworkCore;
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

        _ = await this.context.TodoLists.FindAsync(taskItemEntity.TodoListId)
            ?? throw new KeyNotFoundException($"To-do list (id = {taskItemEntity.TodoListId}) not found.");

        var createdTask = await this.context.Tasks.AddAsync(taskItemEntity);
        _ = await this.context.SaveChangesAsync();

        return createdTask.Entity;
    }

    public async Task DeleteAsync(int id, int todoListId, string ownerId)
    {
        _ = await this.context.TodoLists.FindAsync(todoListId)
            ?? throw new KeyNotFoundException($"To-do list (id = {todoListId}) not found.");

        var task = await this.context.Tasks
            .Where(t => t.TodoListId == todoListId && t.OwnerId == ownerId)
            .FirstOrDefaultAsync(t => t.Id == id)
            ?? throw new KeyNotFoundException($"Task (id = {id}) not found.");

        _ = this.context.Tasks.Remove(task);
        _ = await this.context.SaveChangesAsync();
    }

    public async Task<IEnumerable<TaskItemEntity>> GetAllAsync(int todoListId, string ownerId)
    {
        return await this.context.Tasks
            .AsNoTracking()
            .Where(t => t.TodoListId == todoListId && t.OwnerId == ownerId)
            .ToListAsync();
    }

    public async Task<TaskItemEntity> GetByIdAsync(int id, int todoListId, string ownerId)
    {
        _ = await this.context.TodoLists.FindAsync(todoListId)
            ?? throw new KeyNotFoundException($"To-do list (id = {id}) not found.");

        return await this.context.Tasks
                   .Where(t => t.TodoListId == todoListId && t.OwnerId == ownerId)
                   .FirstOrDefaultAsync(t => t.Id == id)
               ?? throw new KeyNotFoundException($"Task (id = {id}) not found.");
    }

    public async Task UpdateAsync(int id, int todoListId, TaskItemEntity taskItemEntity)
    {
        ArgumentNullException.ThrowIfNull(taskItemEntity);

        _ = await this.context.TodoLists.FindAsync(todoListId)
            ?? throw new KeyNotFoundException($"To-do list (id = {todoListId}) not found.");

        var existingTask = await this.context.Tasks
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id == id)
            ?? throw new KeyNotFoundException($"Task (id = {id}) not found.");

        taskItemEntity.TodoListId = todoListId;
        taskItemEntity.Id = existingTask.Id;

        this.context.Entry(taskItemEntity).State = EntityState.Modified;

        _ = await this.context.SaveChangesAsync();
    }
}
