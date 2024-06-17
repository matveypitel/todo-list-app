using TodoListApp.Models.Domains;
using TodoListApp.Models.DTOs;
using TodoListApp.Models.Enums;

namespace TodoListApp.WebApi.Interfaces;

public interface IShareUserDatabaseService
{
    Task<PagedModel<TodoListUser>> GetPagedTodoListUsersListAsync(int todoListId, string requesterUserName, int page, int pageSize);

    Task<TodoListUser> GetUserByNameAsync(int todoListId, string requesterUserName, string userName);

    Task<TodoListUser> AddUserToTodoListAsync(int todoListId, string requesterUserName, string userName, TodoListRole role);

    Task UpdateUserRoleAsync(int todoListId, string requesterUserName, string userName, TodoListRole newRole);

    Task RemoveUserFromTodoListAsync(int todoListId, string requesterUserName, string userName);
}
