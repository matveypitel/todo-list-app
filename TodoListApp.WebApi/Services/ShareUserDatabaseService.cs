using AutoMapper;
using TodoListApp.Models.Domains;
using TodoListApp.Models.DTOs;
using TodoListApp.Models.Enums;
using TodoListApp.WebApi.Interfaces;

namespace TodoListApp.WebApi.Services;

public class ShareUserDatabaseService : IShareUserDatabaseService
{
    private readonly IShareUserRepository repository;
    private readonly IMapper mapper;

    public ShareUserDatabaseService(IShareUserRepository repository, IMapper mapper)
    {
        this.repository = repository;
        this.mapper = mapper;
    }

    public async Task<PagedModel<TodoListUser>> GetPagedTodoListUsersListAsync(int todoListId, string requesterUserName, int page, int pageSize)
    {
        var usersList = await this.repository.GetPagedListAsync(todoListId, requesterUserName, page, pageSize);
        return this.mapper.Map<PagedModel<TodoListUser>>(usersList);
    }

    public async Task<TodoListUser> GetUserByNameAsync(int todoListId, string requesterUserName, string userName)
    {
        var todoListUser = await this.repository.GetByNameAsync(todoListId, requesterUserName, userName);
        return this.mapper.Map<TodoListUser>(todoListUser);
    }

    public async Task<TodoListUser> AddUserToTodoListAsync(int todoListId, string requesterUserName, string userName, TodoListRole role)
    {
        var todoListUser = await this.repository.AddUserToTodoListAsync(todoListId, requesterUserName, userName, role);
        return this.mapper.Map<TodoListUser>(todoListUser);
    }

    public async Task UpdateUserRoleAsync(int todoListId, string requesterUserName, string userName, TodoListRole newRole)
    {
        await this.repository.UpdateUserRoleAsync(todoListId, requesterUserName, userName, newRole);
    }

    public async Task RemoveUserFromTodoListAsync(int todoListId, string requesterUserName, string userName)
    {
        await this.repository.RemoveUserFromTodoListAsync(todoListId, requesterUserName, userName);
    }
}
