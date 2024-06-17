using TodoListApp.Models.Domains;
using TodoListApp.Models.DTOs;
using TodoListApp.Models.Enums;

namespace TodoListApp.WebApp.Interfaces;

public interface IShareUserWebApiService
{
    Task<PagedModel<TodoListUser>> GetPagedListOfUsersInTodoListAsync(string token, int todoListId, int page, int pageSize);

    Task<TodoListUser> GetUserInTodoListAsync(string token, int todoListId, string userName);

    Task<TodoListUser> AddUserToTodoListAsync(string token, int todoListId, TodoListUser user);

    Task UpdateUserRoleAsync(string token, int todoListId, string userName, TodoListUser user);

    Task RemoveUserFromTodoListAsync(string token, int todoListId, string userName);
}
