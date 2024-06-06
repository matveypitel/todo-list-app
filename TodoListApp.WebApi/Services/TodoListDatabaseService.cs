using AutoMapper;
using TodoListApp.Models.Domains;
using TodoListApp.WebApi.Abstractions;
using TodoListApp.WebApi.Data.Entities;

namespace TodoListApp.WebApi.Services;

public class TodoListDatabaseService : ITodoListDatabaseService
{
    private readonly IMapper mapper;
    private readonly ITodoListRepository repository;

    public TodoListDatabaseService(IMapper mapper, ITodoListRepository repository)
    {
        this.repository = repository;
        this.mapper = mapper;
    }

    public async Task<TodoList> CreateAsync(TodoList todoList)
    {
        var todoListEntity = await this.repository.CreateAsync(this.mapper.Map<TodoListEntity>(todoList));
        return this.mapper.Map<TodoList>(todoListEntity);
    }

    public async Task<IEnumerable<TodoList>> GetAllAsync(string userId, int pageNumber, int pageSize)
    {
        var todoListEntities = await this.repository.GetAllAsync(userId, pageNumber, pageSize);
        return this.mapper.Map<IEnumerable<TodoList>>(todoListEntities);
    }

    public async Task<TodoList> GetByIdAsync(int id, string userId)
    {
        var todoListEntity = await this.repository.GetByIdAsync(id, userId);
        return this.mapper.Map<TodoList>(todoListEntity);
    }

    public async Task UpdateAsync(int id, TodoList todoList)
    {
        var todoListEntity = this.mapper.Map<TodoListEntity>(todoList);
        await this.repository.UpdateAsync(id, todoListEntity);
    }

    public async Task DeleteAsync(int id, string userId)
    {
        await this.repository.DeleteAsync(id, userId);
    }

    public async Task<int> GetCountAsync(string userId)
    {
        return await this.repository.GetCountAsync(userId);
    }
}
