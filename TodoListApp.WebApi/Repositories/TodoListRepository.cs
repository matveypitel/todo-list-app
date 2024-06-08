using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TodoListApp.Models.DTOs;
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

    public async Task<PagedModel<TodoListEntity>> GetPagedListAsync(string userId, int page, int pageSize)
    {
        var listOfTodoList = await this.context.TodoLists
           .Where(t => t.UserId == userId)
           .Skip((page - 1) * pageSize)
           .Take(pageSize)
           .AsNoTracking()
           .ToListAsync();

        var totalCount = await this.context.TodoLists
           .Where(t => t.UserId == userId)
           .CountAsync();

        var pagedModel = new PagedModel<TodoListEntity>
        {
            Items = listOfTodoList,
            CurrentPage = page,
            ItemsPerPage = pageSize,
            TotalCount = totalCount,
        };

        return pagedModel;
    }

    public async Task<TodoListEntity> GetByIdAsync(int id, string userId)
    {
        return await this.context.TodoLists
            .Where(t => t.UserId == userId)
            .Include(t => t.Tasks)
            .FirstOrDefaultAsync(t => t.Id == id)
            ?? throw new KeyNotFoundException($"To-do list (id = {id}) not found.");
    }

    public async Task UpdateAsync(int id, TodoListEntity todoListEntity)
    {
        ArgumentNullException.ThrowIfNull(todoListEntity);

        var existingTodoList = await this.context.TodoLists
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id == id)
            ?? throw new KeyNotFoundException($"To-do list (id = {id}) not found.");

        todoListEntity.Id = existingTodoList.Id;

        this.context.Entry(todoListEntity).State = EntityState.Modified;

        _ = await this.context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id, string userId)
    {
        var todoList = await this.context.TodoLists
            .Where(t => t.UserId == userId)
            .FirstOrDefaultAsync(t => t.Id == id)
            ?? throw new KeyNotFoundException($"To-do list (id = {id}) not found.");

        _ = this.context.TodoLists.Remove(todoList);
        _ = await this.context.SaveChangesAsync();
    }
}
