using AutoMapper;
using TodoListApp.Models.Domains;
using TodoListApp.Models.DTOs;
using TodoListApp.WebApi.Data.Entities;
using TodoListApp.WebApi.Interfaces;

namespace TodoListApp.WebApi.Services;

/// <summary>
/// Represents a service for interacting with the TodoList database.
/// </summary>
public class TodoListDatabaseService : ITodoListDatabaseService
{
    private readonly IMapper mapper;
    private readonly ITodoListRepository repository;

    /// <summary>
    /// Initializes a new instance of the <see cref="TodoListDatabaseService"/> class.
    /// </summary>
    /// <param name="context">The TodoListDbContext.</param>
    public TodoListDatabaseService(IMapper mapper, ITodoListRepository repository)
    {
        this.repository = repository;
        this.mapper = mapper;
    }

    /// <inheritdoc/>
    public async Task<TodoList> CreateTodoListAsync(TodoList todoList, string userName)
    {
        var todoListEntity = await this.repository.CreateAsync(this.mapper.Map<TodoListEntity>(todoList), userName);
        return this.mapper.Map<TodoList>(todoListEntity);
    }

    /// <inheritdoc/>
    public async Task<PagedModel<TodoList>> GetPagedListOfTodoListsAsync(string userName, int page, int pageSize)
    {
        var todoListEntities = await this.repository.GetPagedListAsync(userName, page, pageSize);
        return this.mapper.Map<PagedModel<TodoList>>(todoListEntities);
    }

    /// <inheritdoc/>
    public async Task<TodoList> GetTodoListByIdAsync(int id, string userName)
    {
        var todoListEntity = await this.repository.GetByIdAsync(id, userName);
        return this.mapper.Map<TodoList>(todoListEntity);
    }

    /// <inheritdoc/>
    public async Task UpdateTodoListAsync(int id, TodoList todoList, string userName)
    {
        var todoListEntity = this.mapper.Map<TodoListEntity>(todoList);
        await this.repository.UpdateAsync(id, todoListEntity, userName);
    }

    /// <inheritdoc/>
    public async Task DeleteTodoListAsync(int id, string userName)
    {
        await this.repository.DeleteAsync(id, userName);
    }
}
