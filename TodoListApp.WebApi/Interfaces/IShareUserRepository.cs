using TodoListApp.Models.DTOs;
using TodoListApp.Models.Enums;
using TodoListApp.WebApi.Data.Entities;

namespace TodoListApp.WebApi.Interfaces;

public interface IShareUserRepository
{
    Task<PagedModel<TodoListUserEntity>> GetPagedListAsync(int todoListId, string requesterUserName, int page, int pageSize);

    Task<TodoListUserEntity> GetByNameAsync(int todoListId, string requesterUserName, string userName);

    Task<TodoListUserEntity> AddUserToTodoListAsync(int todoListId, string requesterUserName, string userName, TodoListRole role);

    Task UpdateUserRoleAsync(int todoListId, string requesterUserName, string userName, TodoListRole newRole);

    Task RemoveUserFromTodoListAsync(int todoListId, string requesterUserName, string userName);
}
