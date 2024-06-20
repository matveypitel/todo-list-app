using Microsoft.EntityFrameworkCore;
using TodoListApp.Models.DTOs;
using TodoListApp.Models.Enums;
using TodoListApp.WebApi.Data;
using TodoListApp.WebApi.Data.Entities;
using TodoListApp.WebApi.Interfaces;

namespace TodoListApp.WebApi.Repositories;

public class TagRepository : ITagRepository
{
    private readonly TodoListDbContext context;

    public TagRepository(TodoListDbContext context)
    {
        this.context = context;
    }

    public async Task<TagEntity> AddToTaskAsync(int taskId, TagEntity tagEntity, string userName)
    {
        ArgumentNullException.ThrowIfNull(tagEntity);

        if (!await this.UserHasOwnerOrEditorPermissionAsync(taskId, userName))
        {
            throw new UnauthorizedAccessException("User does not have permission to add tags.");
        }

        var taskEntity = await this.context.Tasks
                .Include(t => t.Tags)
                .FirstOrDefaultAsync(t => t.Id == taskId)
                ?? throw new KeyNotFoundException($"Task (id = {taskId}) not found.");

        var existingTagInTask = taskEntity.Tags.FirstOrDefault(t => t.Label == tagEntity.Label);
        if (existingTagInTask != null)
        {
            throw new DbUpdateException($"Tag (label = {tagEntity.Label}) already exists.");
        }

        taskEntity.Tags.Add(tagEntity);
        _ = await this.context.SaveChangesAsync();

        return tagEntity;
    }

    public async Task DeleteAsync(int id, int taskId, string userName)
    {
        if (!await this.UserHasOwnerOrEditorPermissionAsync(taskId, userName))
        {
            throw new UnauthorizedAccessException("User does not have permission to delete tags.");
        }

        var taskEntity = await this.context.Tasks
                .Include(t => t.Tags)
                .FirstOrDefaultAsync(t => t.Id == taskId)
                ?? throw new KeyNotFoundException($"Task (id = {taskId}) not found.");

        var tag = await this.context.Tags
            .Where(t => t.Tasks.Any(task => task.Id == taskId) && t.Id == id)
            .FirstOrDefaultAsync()
            ?? throw new KeyNotFoundException($"Tag (id = {id}) not found.");

        _ = taskEntity.Tags.Remove(tag);
        _ = await this.context.SaveChangesAsync();
    }

    public async Task<TagEntity> GetByIdAsync(int id, int taskId, string userName)
    {
        if (!await this.UserHasOwnerOrEditorPermissionAsync(taskId, userName))
        {
            throw new UnauthorizedAccessException("User does not have permission to access this tag.");
        }

        await this.TagTaskExistsAsync(taskId);

        return await this.context.Tags
            .Where(t => t.Tasks.Any(task => task.Id == taskId) && t.Id == id)
            .FirstOrDefaultAsync()
            ?? throw new KeyNotFoundException($"Tag (id = {id}) not found.");
    }

    public async Task<PagedModel<TagEntity>> GetPagedListOfAllAsync(string userName, int page, int pageSize)
    {
        var accessibleTodoListIds = await this.context.TodoLists
            .Where(tl => tl.Users.Any(u => u.UserName == userName))
            .Select(tl => tl.Id)
            .ToListAsync();

        var tasks = await this.context.Tasks
            .Where(task => accessibleTodoListIds.Contains(task.TodoListId))
            .Include(task => task.Tags)
            .ToListAsync();

        var tagsWithTasks = tasks
            .SelectMany(task => task.Tags, (task, tag) => new { task, tag })
            .GroupBy(t => t.tag.Label)
            .Select(g => new TagEntity
            {
                Label = g.Key,
                Tasks = g.Select(t => t.task).ToList(),
            })
            .ToList();

        var pagedTags = tagsWithTasks
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        return new PagedModel<TagEntity>
        {
            Items = pagedTags,
            TotalCount = tagsWithTasks.Count,
            CurrentPage = page,
            ItemsPerPage = pageSize,
        };
    }

    private async Task TagTaskExistsAsync(int taskId)
    {
        _ = await this.context.Tasks
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id == taskId)
            ?? throw new KeyNotFoundException($"Task (id = {taskId}) not found.");
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

        return userRole == TodoListRole.Editor || userRole == TodoListRole.Owner;
    }
}
