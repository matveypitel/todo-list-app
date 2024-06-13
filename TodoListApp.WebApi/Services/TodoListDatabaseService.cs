using AutoMapper;
using TodoListApp.Models.Domains;
using TodoListApp.Models.DTOs;
using TodoListApp.WebApi.Interfaces;
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

    public async Task<PagedModel<TodoList>> GetPagedListOfTodoListsAsync(string ownerName, int page, int pageSize)
    {
        var todoListEntities = await this.repository.GetPagedListAsync(ownerName, page, pageSize);
        return this.mapper.Map<PagedModel<TodoList>>(todoListEntities);
    }

    public async Task<TodoList> GetTodoListByIdAsync(int id, string ownerName)
    {
        var todoListEntity = await this.repository.GetByIdAsync(id, ownerName);
        return this.mapper.Map<TodoList>(todoListEntity);
    }

    public async Task UpdateTodoListAsync(int id, TodoList todoList)
    {
        var todoListEntity = this.mapper.Map<TodoListEntity>(todoList);
        await this.repository.UpdateAsync(id, todoListEntity);
    }

    public async Task DeleteTodoListAsync(int id, string ownerName)
    {
        await this.repository.DeleteAsync(id, ownerName);
    }
}
