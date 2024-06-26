using Microsoft.EntityFrameworkCore;
using TodoListApp.Models.DTOs;
using TodoListApp.Models.Enums;
using TodoListApp.WebApi.Data;
using TodoListApp.WebApi.Data.Entities;
using TodoListApp.WebApi.Interfaces;

namespace TodoListApp.WebApi.Repositories;

/// <inheritdoc/>
public class ShareUserRepository : IShareUserRepository
{
    private readonly TodoListDbContext context;

    /// <summary>
    /// Initializes a new instance of the <see cref="ShareUserRepository"/> class.
    /// </summary>
    /// <param name="context">The TodoListDbContext.</param>
    public ShareUserRepository(TodoListDbContext context)
    {
        this.context = context;
    }

    /// <inheritdoc/>
    public async Task<PagedModel<TodoListUserEntity>> GetPagedListAsync(int todoListId, string requesterUserName, int page, int pageSize)
    {
        await this.IsOwner(todoListId, requesterUserName);

        var query = this.context.TodoListsUsers
            .Where(tlu => tlu.TodoListId == todoListId);

        var totalItems = await query.CountAsync();
        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedModel<TodoListUserEntity>
        {
            Items = items,
            TotalCount = totalItems,
            CurrentPage = page,
            ItemsPerPage = pageSize,
        };
    }

    /// <inheritdoc/>
    public async Task<TodoListUserEntity> GetByNameAsync(int todoListId, string requesterUserName, string userName)
    {
        await this.IsOwner(todoListId, requesterUserName);

        return await this.context.TodoListsUsers
            .FirstOrDefaultAsync(tlu => tlu.TodoListId == todoListId && tlu.UserName == userName)
            ?? throw new KeyNotFoundException("The user is not associated with the TodoList.");
    }

    /// <inheritdoc/>
    public async Task<TodoListUserEntity> AddUserToTodoListAsync(int todoListId, string requesterUserName, string userName, TodoListRole role)
    {
        await this.IsOwner(todoListId, requesterUserName);

        if (await this.context.TodoListsUsers.AnyAsync(tlu => tlu.TodoListId == todoListId && tlu.UserName == userName))
        {
            throw new DbUpdateException("The user is already associated with the TodoList.");
        }

        var todoListUser = new TodoListUserEntity
        {
            TodoListId = todoListId,
            UserName = userName,
            Role = role,
        };

        _ = await this.context.TodoListsUsers.AddAsync(todoListUser);
        _ = await this.context.SaveChangesAsync();

        return todoListUser;
    }

    /// <inheritdoc/>
    public async Task UpdateUserRoleAsync(int todoListId, string requesterUserName, string userName, TodoListRole newRole)
    {
        await this.IsOwner(todoListId, requesterUserName);

        var todoListUser = await this.context.TodoListsUsers
            .FirstOrDefaultAsync(tlu => tlu.TodoListId == todoListId && tlu.UserName == userName)
            ?? throw new KeyNotFoundException("The user is not associated with the TodoList.");

        todoListUser.Role = newRole;
        this.context.Entry(todoListUser).State = EntityState.Modified;

        _ = await this.context.SaveChangesAsync();
    }

    /// <inheritdoc/>
    public async Task RemoveUserFromTodoListAsync(int todoListId, string requesterUserName, string userName)
    {
        await this.IsOwner(todoListId, requesterUserName);

        var todoListUser = await this.context.TodoListsUsers
            .FirstOrDefaultAsync(tlu => tlu.TodoListId == todoListId && tlu.UserName == userName)
            ?? throw new KeyNotFoundException("The user is not associated with the TodoList.");

        _ = this.context.TodoListsUsers.Remove(todoListUser);
        _ = await this.context.SaveChangesAsync();
    }

    private async Task IsOwner(int todoListId, string requesterUserName)
    {
        var isOwner = await this.context.TodoListsUsers
            .AnyAsync(tlu => tlu.TodoListId == todoListId && tlu.UserName == requesterUserName && tlu.Role == TodoListRole.Owner);

        if (!isOwner)
        {
            throw new UnauthorizedAccessException("Only the owner can remove users from the TodoList.");
        }
    }
}
