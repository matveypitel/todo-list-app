using Microsoft.EntityFrameworkCore;
using TodoListApp.Models.DTOs;
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

    public async Task<TagEntity> AddToTaskAsync(int taskId, TagEntity tagEntity)
    {
        ArgumentNullException.ThrowIfNull(tagEntity);

        var taskEntity = await this.context.Tasks
                .Include(t => t.Tags)
                .FirstOrDefaultAsync(t => t.Id == taskId)
                ?? throw new KeyNotFoundException($"Task (id = {taskId}) not found.");

        var existingTagInTask = taskEntity.Tags.FirstOrDefault(t => t.Label == tagEntity.Label);
        if (existingTagInTask != null)
        {
            // TODO: create custom exception
            throw new ArgumentNullException($"Tag (label = {tagEntity.Label}) already exists.");
        }

        taskEntity.Tags.Add(tagEntity);
        _ = await this.context.SaveChangesAsync();

        return tagEntity;
    }

    public async Task DeleteAsync(int id, int taskId)
    {
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

    public async Task<TagEntity> GetByIdAsync(int id, int taskId)
    {
        await this.TagTaskExistsAsync(taskId);

        return await this.context.Tags
            .Where(t => t.Tasks.Any(task => task.Id == taskId) && t.Id == id)
            .Include(t => t.Tasks)
            .FirstOrDefaultAsync()
            ?? throw new KeyNotFoundException($"Tag (id = {id}) not found.");
    }

    public async Task<PagedModel<TagEntity>> GetPagedListOfAllAsync(string tasksOwnerName, int page, int pageSize)
    {
        var tasks = await this.context.Tasks
            .Where(task => task.Owner == tasksOwnerName || task.AssignedTo == tasksOwnerName)
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

    public async Task UpdateAsync(int id, int taskId, TagEntity tagEntity)
    {
        ArgumentNullException.ThrowIfNull(tagEntity);

        await this.TagTaskExistsAsync(taskId);

        var existingTag = await this.context.Tags
            .Where(t => t.Id == id && t.Tasks.Any(task => task.Id == taskId))
            .FirstOrDefaultAsync()
            ?? throw new KeyNotFoundException($"Tag (id = {id}) not found for task (id = {taskId}).");

        existingTag.Label = tagEntity.Label;

        this.context.Entry(existingTag).State = EntityState.Modified;

        _ = await this.context.SaveChangesAsync();
    }

    public async Task TagTaskExistsAsync(int taskId)
    {
        _ = await this.context.Tasks
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id == taskId)
            ?? throw new KeyNotFoundException($"Task (id = {taskId}) not found.");
    }
}
