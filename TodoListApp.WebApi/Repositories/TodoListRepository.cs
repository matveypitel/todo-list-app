using Microsoft.EntityFrameworkCore;
using TodoListApp.Models.DTOs;
using TodoListApp.Models.Enums;
using TodoListApp.WebApi.Data;
using TodoListApp.WebApi.Data.Entities;
using TodoListApp.WebApi.Interfaces;

namespace TodoListApp.WebApi.Repositories;

/// <summary>
/// Represents a repository for managing to-do lists.
/// </summary>
public class TodoListRepository : ITodoListRepository
{
    private readonly TodoListDbContext context;

    /// <summary>
    /// Initializes a new instance of the <see cref="TodoListRepository"/> class.
    /// </summary>
    /// <param name="context">The TodoListDbContext.</param>
    public TodoListRepository(TodoListDbContext context)
    {
        this.context = context;
    }

    /// <inheritdoc />
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="todoListEntity"/> is null.</exception>
    public async Task<TodoListEntity> CreateAsync(TodoListEntity todoListEntity, string userName)
    {
        var createdEntity = await this.context.TodoLists.AddAsync(todoListEntity);

        _ = await this.context.SaveChangesAsync();

        var todoListUser = new TodoListUserEntity
        {
            UserName = userName,
            TodoListId = createdEntity.Entity.Id,
            Role = TodoListRole.Owner,
        };

        _ = await this.context.TodoListsUsers.AddAsync(todoListUser);
        _ = await this.context.SaveChangesAsync();

        return createdEntity.Entity;
    }

    /// <inheritdoc />
    /// <exception cref="KeyNotFoundException">Thrown when the to-do list with the specified ID is not found.</exception>
    public async Task<PagedModel<TodoListEntity>> GetPagedListAsync(string userName, int page, int pageSize)
    {
        var query = this.context.TodoLists
            .Where(t => t.Users.Any(u => u.UserName == userName))
            .Include(u => u.Users);

        var totalCount = await query.CountAsync();

        var listOfTodoList = await query
            .OrderByDescending(t => t.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .AsNoTracking()
            .ToListAsync();

        return new PagedModel<TodoListEntity>
        {
            Items = listOfTodoList,
            CurrentPage = page,
            ItemsPerPage = pageSize,
            TotalCount = totalCount,
        };
    }

    /// <inheritdoc />
    /// <exception cref="KeyNotFoundException">Thrown when the to-do list with the specified ID is not found.</exception>
    public async Task<TodoListEntity> GetByIdAsync(int id, string userName)
    {
        return await this.context.TodoLists
            .Include(u => u.Users)
            .FirstOrDefaultAsync(t => t.Id == id && t.Users.Any(u => u.UserName == userName))
            ?? throw new KeyNotFoundException($"To-do list (id = {id}) not found.");
    }

    /// <inheritdoc />
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="todoListEntity"/> is null.</exception>
    /// <exception cref="KeyNotFoundException">Thrown when the to-do list with the specified ID is not found or access is denied.</exception>
    public async Task UpdateAsync(int id, TodoListEntity todoListEntity, string userName)
    {
        ArgumentNullException.ThrowIfNull(todoListEntity);

        if (await this.IsViewer(id, userName))
        {
            throw new UnauthorizedAccessException("Viewer can't update to-do lists.");
        }

        var existingTodoList = await this.context.TodoLists
            .Include(u => u.Users)
            .FirstOrDefaultAsync(t => t.Id == id && t.Users.Any(u => u.UserName == userName))
            ?? throw new KeyNotFoundException($"TodoList with ID {id} not found or access denied.");

        existingTodoList.Title = todoListEntity.Title;
        existingTodoList.Description = todoListEntity.Description;

        _ = await this.context.SaveChangesAsync();
    }

    /// <inheritdoc />
    /// <exception cref="KeyNotFoundException">Thrown when the to-do list with the specified ID is not found or access is denied.</exception>
    public async Task DeleteAsync(int id, string userName)
    {
        if (!await this.IsOwner(id, userName))
        {
            throw new UnauthorizedAccessException("Only the owner can remove to-do lists.");
        }

        var todoList = await this.context.TodoLists
            .FirstOrDefaultAsync(t => t.Id == id && t.Users.Any(u => u.UserName == userName))
            ?? throw new KeyNotFoundException($"TodoList with ID {id} not found or access denied.");

        _ = this.context.TodoLists.Remove(todoList);
        _ = await this.context.SaveChangesAsync();
    }

    private async Task<bool> IsOwner(int todoListId, string requesterUserName)
    {
        return await this.context.TodoListsUsers
            .AnyAsync(tlu => tlu.TodoListId == todoListId && tlu.UserName == requesterUserName && tlu.Role == TodoListRole.Owner);
    }

    private async Task<bool> IsViewer(int todoListId, string requesterUserName)
    {
        return await this.context.TodoListsUsers
            .AnyAsync(tlu => tlu.TodoListId == todoListId && tlu.UserName == requesterUserName && tlu.Role == TodoListRole.Viewer);
    }
}
