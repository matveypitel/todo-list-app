using System.Globalization;
using Microsoft.EntityFrameworkCore;
using TodoListApp.Models.DTOs;
using TodoListApp.Models.Enums;
using TodoListApp.WebApi.Data;
using TodoListApp.WebApi.Data.Entities;
using TodoListApp.WebApi.Interfaces;

namespace TodoListApp.WebApi.Repositories;

public class TaskRepository : ITaskRepository
{
    private readonly TodoListDbContext context;

    public TaskRepository(TodoListDbContext context)
    {
        this.context = context;
    }

    public async Task<TodoListRole> GetUserRoleInTodoListAsync(int todoListId, string userName)
    {
        return await this.context.TodoListsUsers
            .Where(tlu => tlu.TodoListId == todoListId && tlu.UserName == userName)
            .Select(tlu => tlu.Role)
            .FirstOrDefaultAsync();
    }

    public async Task<TaskItemEntity> CreateAsync(TaskItemEntity taskItemEntity, string userName)
    {
        ArgumentNullException.ThrowIfNull(taskItemEntity);

        if (!await this.IsOwner(taskItemEntity.TodoListId, userName))
        {
            throw new UnauthorizedAccessException("Only the owner can remove users from the TodoList.");
        }

        _ = await this.context.TodoLists
            .SingleOrDefaultAsync(t => t.Id == taskItemEntity.TodoListId && t.Users.Any(u => u.UserName == userName && u.Role == TodoListRole.Owner))
            ?? throw new KeyNotFoundException($"To-do list (id = {taskItemEntity.TodoListId}) not found.");

        var createdTask = await this.context.Tasks.AddAsync(taskItemEntity);
        _ = await this.context.SaveChangesAsync();

        return createdTask.Entity;
    }

    public async Task<PagedModel<TaskItemEntity>> GetPagedListAsync(int todoListId, string userName, int page, int pageSize)
    {
        _ = await this.context.TodoLists
            .Where(t => t.Users.Any(u => u.UserName == userName))
            .FirstOrDefaultAsync(t => t.Id == todoListId)
            ?? throw new KeyNotFoundException($"To-do list (id = {todoListId}) not found.");

        var tasks = await this.context.Tasks
            .AsNoTracking()
            .OrderByDescending(t => t.CreatedDate)
            .Where(t => t.TodoListId == todoListId)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var totalCount = await this.context.Tasks
           .Where(t => t.TodoListId == todoListId)
           .CountAsync();

        return new PagedModel<TaskItemEntity>
        {
            Items = tasks,
            CurrentPage = page,
            ItemsPerPage = pageSize,
            TotalCount = totalCount,
        };
    }

    public async Task<PagedModel<TaskItemEntity>> GetPagedListWithTagAsync(string userName, string tagLabel, int page, int pageSize)
    {
        var accessibleTodoListIds = await this.context.TodoLists
            .Where(tl => tl.Users.Any(u => u.UserName == userName))
            .Select(tl => tl.Id)
            .ToListAsync();

        var tasksQuery = this.context.Tasks
            .AsNoTracking()
            .Where(task => task.Tags.Any(tag => tag.Label == tagLabel) && accessibleTodoListIds.Contains(task.TodoListId));

        var tasks = await tasksQuery
            .OrderByDescending(t => t.CreatedDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var totalCount = await tasksQuery.CountAsync();

        return new PagedModel<TaskItemEntity>
        {
            Items = tasks,
            CurrentPage = page,
            ItemsPerPage = pageSize,
            TotalCount = totalCount,
        };
    }

    public async Task<PagedModel<TaskItemEntity>> GetPagedListOfAssignedToUserAsync(string userName, int page, int pageSize, string? status, string? sort)
    {
        var tasks = this.context.Tasks
            .Where(t => t.AssignedTo == userName)
            .OrderByDescending(t => t.CreatedDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .AsNoTracking();

        if (string.IsNullOrEmpty(status))
        {
            tasks = tasks.Where(t => t.Status != TaskItemStatus.Completed);
        }

        if (!string.IsNullOrEmpty(status) && status != "All")
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

        var totalCount = await tasks.CountAsync();
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

    public async Task<PagedModel<TaskItemEntity>> GetPagedListOfSearchResultsAsync(string userName, string? title, string? creationDate, string? dueDate, int page, int pageSize)
    {
        var accessibleTodoListIds = await this.context.TodoLists
        .Where(tl => tl.Users.Any(u => u.UserName == userName))
        .Select(tl => tl.Id)
        .ToListAsync();

        var tasksQuery = this.context.Tasks
            .AsNoTracking()
            .Where(task => accessibleTodoListIds.Contains(task.TodoListId));

        if (!string.IsNullOrEmpty(title))
        {
            tasksQuery = tasksQuery.Where(t => t.Title.Contains(title));
        }

        if (!string.IsNullOrEmpty(creationDate))
        {
            tasksQuery = tasksQuery.Where(t => t.CreatedDate.Date.ToString(CultureInfo.InvariantCulture) == creationDate);
        }

        if (!string.IsNullOrEmpty(dueDate))
        {
            tasksQuery = tasksQuery.Where(t => t.DueDate.HasValue && t.DueDate.Value.ToString(CultureInfo.InvariantCulture) == dueDate);
        }

        var tasks = await tasksQuery
            .OrderByDescending(t => t.CreatedDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var totalCount = await tasksQuery.CountAsync();

        return new PagedModel<TaskItemEntity>
        {
            Items = tasks,
            CurrentPage = page,
            ItemsPerPage = pageSize,
            TotalCount = totalCount,
        };
    }

    public async Task<TaskItemEntity> GetByIdAsync(int id, int todoListId, string userName)
    {
        _ = await this.context.TodoLists
            .Where(t => t.Users.Any(u => u.UserName == userName))
            .FirstOrDefaultAsync(t => t.Id == todoListId)
            ?? throw new KeyNotFoundException($"To-do list (id = {todoListId}) not found.");

        return await this.context.Tasks
            .Where(t => t.TodoListId == todoListId)
            .Include(t => t.Tags)
            .Include(t => t.Comments)
            .FirstOrDefaultAsync(t => t.Id == id)
            ?? throw new KeyNotFoundException($"Task (id = {id}) not found.");
    }

    public async Task UpdateAsync(int id, int todoListId, TaskItemEntity taskItemEntity, string userName)
    {
        ArgumentNullException.ThrowIfNull(taskItemEntity);

        if (!await this.IsOwner(todoListId, userName))
        {
            throw new UnauthorizedAccessException("Only the owner can remove users from the TodoList.");
        }

        _ = await this.context.TodoLists
            .Where(t => t.Users.Any(u => u.UserName == taskItemEntity.Owner && u.Role == TodoListRole.Owner))
            .FirstOrDefaultAsync(t => t.Id == todoListId)
            ?? throw new KeyNotFoundException($"To-do list (id = {todoListId}) not found.");

        var existingTask = await this.context.Tasks
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id == id)
            ?? throw new KeyNotFoundException($"Task (id = {id}) not found.");

        taskItemEntity.TodoListId = todoListId;
        taskItemEntity.Id = existingTask.Id;

        if (taskItemEntity.DueDate == null)
        {
            taskItemEntity.DueDate = existingTask.DueDate;
        }

        if (string.IsNullOrEmpty(taskItemEntity.AssignedTo))
        {
            taskItemEntity.AssignedTo = taskItemEntity.Owner;
        }

        this.context.Entry(taskItemEntity).State = EntityState.Modified;

        _ = await this.context.SaveChangesAsync();
    }

    public async Task UpdateTaskStatusAsync(int id, string userName, TaskItemEntity taskItemEntity)
    {
        ArgumentNullException.ThrowIfNull(taskItemEntity);

        var task = await this.context.Tasks
            .AsNoTracking()
            .Where(t => t.AssignedTo == userName)
            .FirstOrDefaultAsync(t => t.Id == id)
            ?? throw new KeyNotFoundException($"Task (id = {id}) not found.");

        task.Status = Enum.Parse<TaskItemStatus>(taskItemEntity.Status.ToString(), true);

        this.context.Entry(task).State = EntityState.Modified;

        _ = await this.context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id, int todoListId, string userName)
    {
        if (!await this.IsOwner(todoListId, userName))
        {
            throw new UnauthorizedAccessException("Only the owner can remove users from the TodoList.");
        }

        _ = await this.context.TodoLists
            .Where(t => t.Users.Any(u => u.UserName == userName && u.Role == TodoListRole.Owner))
            .FirstOrDefaultAsync(t => t.Id == todoListId)
            ?? throw new KeyNotFoundException($"To-do list (id = {todoListId}) not found.");

        var task = await this.context.Tasks
            .Where(t => t.TodoListId == todoListId && t.Owner == userName)
            .FirstOrDefaultAsync(t => t.Id == id)
            ?? throw new KeyNotFoundException($"Task (id = {id}) not found.");

        _ = this.context.Tasks.Remove(task);
        _ = await this.context.SaveChangesAsync();
    }

    public async Task<TaskItemEntity> GetAssignedByIdAsync(int id, string userName)
    {
        return await this.context.Tasks
            .Where(t => t.AssignedTo == userName)
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id == id)
            ?? throw new KeyNotFoundException($"Task (id = {id}) not found.");
    }

    private async Task<bool> IsOwner(int todoListId, string requesterUserName)
    {
        return await this.context.TodoListsUsers
            .AnyAsync(tlu => tlu.TodoListId == todoListId && tlu.UserName == requesterUserName && tlu.Role == TodoListRole.Owner);
    }
}
