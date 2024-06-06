using Microsoft.EntityFrameworkCore;
using TodoListApp.WebApi.Abstractions;
using TodoListApp.WebApi.Data;
using TodoListApp.WebApi.Data.Entities;

namespace TodoListApp.WebApi.Repositories;

public class TodoListRepository : ITodoListRepository
{
    private readonly TodoListDbContext context;

    public TodoListRepository(TodoListDbContext context)
    {
        this.context = context;
    }

    public async Task<TodoListEntity> CreateAsync(TodoListEntity todoListEntity)
    {
        var createdEntity = await this.context.TodoLists.AddAsync(todoListEntity);
        _ = await this.context.SaveChangesAsync();
        return createdEntity.Entity;
    }

    public async Task<IEnumerable<TodoListEntity>> GetAllAsync(string userId, int pageNumber, int pageSize)
    {
        return await this.context.TodoLists
           .Where(t => t.UserId == userId)
           .Skip((pageNumber - 1) * pageSize)
           .Take(pageSize)
           .AsNoTracking()
           .ToListAsync();
    }

    public async Task<TodoListEntity> GetByIdAsync(int id, string userId)
    {
        return await this.context.TodoLists
            .Where(t => t.UserId == userId)
            .FirstOrDefaultAsync(t => t.Id == id)
            ?? throw new KeyNotFoundException($"To-do list (id = {id}) not found.");
    }

    public async Task UpdateAsync(int id, TodoListEntity todoListEntity)
    {
        ArgumentNullException.ThrowIfNull(todoListEntity);

        _ = await this.context.TodoLists
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id == id)
            ?? throw new KeyNotFoundException($"To-do list (id = {todoListEntity.Id}) not found.");

        this.context.Entry(todoListEntity).State = EntityState.Modified;

        _ = await this.context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id, string userId)
    {
        var todoList = await this.context.TodoLists.FindAsync(id)
            ?? throw new KeyNotFoundException($"To-do list (id = {id}) not found.");

        _ = this.context.TodoLists.Remove(todoList);
        _ = await this.context.SaveChangesAsync();
    }

    public async Task<int> GetCountAsync(string userId)
    {
        return await this.context.TodoLists
           .Where(t => t.UserId == userId)
           .CountAsync();
    }
}
