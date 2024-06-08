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

    public async Task<TodoList> CreateTodoListAsync(TodoList todoList)
    {
        var todoListEntity = await this.repository.CreateAsync(this.mapper.Map<TodoListEntity>(todoList));
        return this.mapper.Map<TodoList>(todoListEntity);
    }

    public async Task<IEnumerable<TodoList>> GetListOfTodoListsAsync(string userId, int page, int pageSize)
    {
        var todoListEntities = await this.repository.GetAllAsync(userId, page, pageSize);
        return this.mapper.Map<IEnumerable<TodoList>>(todoListEntities);
    }

    public async Task<TodoList> GetTodoListByIdAsync(int id, string userId)
    {
        var todoListEntity = await this.repository.GetByIdAsync(id, userId);
        return this.mapper.Map<TodoList>(todoListEntity);
    }

    public async Task UpdateTodoListAsync(int id, TodoList todoList)
    {
        var todoListEntity = this.mapper.Map<TodoListEntity>(todoList);
        await this.repository.UpdateAsync(id, todoListEntity);
    }

    public async Task DeleteTodoListAsync(int id, string userId)
    {
        await this.repository.DeleteAsync(id, userId);
    }

    public async Task<int> GetCountAsync(string userId)
    {
        return await this.repository.GetCountAsync(userId);
    }
}
