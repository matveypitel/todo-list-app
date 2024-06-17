using AutoMapper;
using TodoListApp.Models.Domains;
using TodoListApp.Models.DTOs;
using TodoListApp.Models.Enums;
using TodoListApp.WebApi.Data.Entities;
using TodoListApp.WebApi.Interfaces;

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

    public async Task<TodoList> CreateTodoListAsync(TodoList todoList, string userName)
    {
        var todoListEntity = await this.repository.CreateAsync(this.mapper.Map<TodoListEntity>(todoList), userName);
        return this.mapper.Map<TodoList>(todoListEntity);
    }

    public async Task<PagedModel<TodoList>> GetPagedListOfTodoListsAsync(string userName, int page, int pageSize)
    {
        var todoListEntities = await this.repository.GetPagedListAsync(userName, page, pageSize);
        return this.mapper.Map<PagedModel<TodoList>>(todoListEntities);
    }

    public async Task<TodoList> GetTodoListByIdAsync(int id, string userName)
    {
        var todoListEntity = await this.repository.GetByIdAsync(id, userName);
        return this.mapper.Map<TodoList>(todoListEntity);
    }

    public async Task UpdateTodoListAsync(int id, TodoList todoList, string userName)
    {
        var todoListEntity = this.mapper.Map<TodoListEntity>(todoList);
        await this.repository.UpdateAsync(id, todoListEntity, userName);
    }

    public async Task DeleteTodoListAsync(int id, string userName)
    {
        await this.repository.DeleteAsync(id, userName);
    }
}
