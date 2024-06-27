using Microsoft.EntityFrameworkCore;
using TodoListApp.Models.DTOs;
using TodoListApp.Models.Enums;
using TodoListApp.WebApi.Data;
using TodoListApp.WebApi.Data.Entities;
using TodoListApp.WebApi.Interfaces;

namespace TodoListApp.WebApi.Repositories;

/// <summary>
/// Represents a repository for managing comments.
/// </summary>
public class CommentRepository : ICommentRepository
{
    private readonly TodoListDbContext context;

    /// <summary>
    /// Initializes a new instance of the <see cref="CommentRepository"/> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    public CommentRepository(TodoListDbContext context)
    {
        this.context = context;
    }

    /// <inheritdoc/>
    public async Task<CommentEntity> GetByIdAsync(int id, int taskId)
    {
        await this.CommentTaskExistsAsync(taskId);

        return await this.context.Comments
            .Where(c => c.Id == id && c.TaskId == taskId)
            .FirstOrDefaultAsync()
            ?? throw new KeyNotFoundException($"Comment (id = {taskId}) not found.");
    }

    /// <inheritdoc/>
    public async Task<PagedModel<CommentEntity>> GetPagedListOfAllAsync(int taskId, int page, int pageSize)
    {
        await this.CommentTaskExistsAsync(taskId);

        var query = this.context.Comments
            .Where(c => c.TaskId == taskId);

        var totalItems = await query.CountAsync();
        var comments = await query
            .OrderByDescending(c => c.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedModel<CommentEntity>
        {
            Items = comments,
            TotalCount = totalItems,
            CurrentPage = page,
            ItemsPerPage = pageSize,
        };
    }

    /// <inheritdoc/>
    public async Task<CommentEntity> AddToTaskAsync(CommentEntity commentEntity, string userName)
    {
        ArgumentNullException.ThrowIfNull(commentEntity);

        if (!await this.UserHasOwnerOrEditorPermissionAsync(commentEntity.TaskId, userName))
        {
            throw new UnauthorizedAccessException("User does not have permission to add comments.");
        }

        _ = await this.context.Tasks
            .Where(t => t.Id == commentEntity.TaskId)
            .FirstOrDefaultAsync()
            ?? throw new KeyNotFoundException($"Task (id = {commentEntity.TaskId}) not found.");

        var createdComment = await this.context.Comments.AddAsync(commentEntity);
        _ = await this.context.SaveChangesAsync();

        return createdComment.Entity;
    }

    /// <inheritdoc/>
    public async Task DeleteAsync(int id, int taskId, string userName)
    {
        if (!await this.UserHasOwnerPermissionAsync(taskId, userName))
        {
            throw new UnauthorizedAccessException("User does not have permission to delete comments.");
        }

        await this.CommentTaskExistsAsync(taskId);

        var comment = await this.context.Comments
            .Where(c => c.Id == id && c.TaskId == taskId)
            .FirstOrDefaultAsync()
            ?? throw new KeyNotFoundException($"Comment (id = {taskId}) not found.");

        _ = this.context.Comments.Remove(comment);
        _ = await this.context.SaveChangesAsync();
    }

    /// <inheritdoc/>
    public async Task UpdateAsync(int id, int taskId, CommentEntity commentEntity, string userName)
    {
        ArgumentNullException.ThrowIfNull(commentEntity);

        if (!await this.UserHasOwnerPermissionAsync(taskId, userName))
        {
            throw new UnauthorizedAccessException("User does not have permission to update comments.");
        }

        await this.CommentTaskExistsAsync(taskId);

        var comment = await this.context.Comments
            .Where(c => c.Id == id && c.TaskId == taskId)
            .FirstOrDefaultAsync()
            ?? throw new KeyNotFoundException($"Comment (id = {taskId}) not found.");

        comment.Text = commentEntity.Text;
        this.context.Entry(comment).State = EntityState.Modified;

        _ = await this.context.SaveChangesAsync();
    }

    private async Task CommentTaskExistsAsync(int taskId)
    {
        _ = await this.context.Tasks
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id == taskId)
            ?? throw new KeyNotFoundException($"Task (id = {taskId}) not found.");
    }

    private async Task<bool> UserHasOwnerPermissionAsync(int taskId, string userName)
    {
        var task = await this.context.Tasks
            .Include(t => t.TodoList)
            .ThenInclude(tl => tl.Users)
            .FirstOrDefaultAsync(t => t.Id == taskId)
            ?? throw new KeyNotFoundException($"Task (id = {taskId}) not found.");

        var userRole = task.TodoList.Users
            .FirstOrDefault(u => u.UserName == userName)?.Role;

        return userRole == TodoListRole.Owner;
    }

    private async Task<bool> UserHasOwnerOrEditorPermissionAsync(int taskId, string userName)
    {
        var task = await this.context.Tasks
            .Include(t => t.TodoList)
            .ThenInclude(tl => tl.Users)
            .FirstOrDefaultAsync(t => t.Id == taskId)
            ?? throw new KeyNotFoundException($"Task (id = {taskId}) not found.");

        var userRole = task.TodoList.Users
            .FirstOrDefault(u => u.UserName == userName)?.Role;

        return userRole == TodoListRole.Owner || userRole == TodoListRole.Editor;
    }
}
