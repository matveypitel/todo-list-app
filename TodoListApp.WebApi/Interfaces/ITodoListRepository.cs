using TodoListApp.Models.DTOs;
using TodoListApp.Models.Enums;
using TodoListApp.WebApi.Data.Entities;

namespace TodoListApp.WebApi.Interfaces;

public interface ITodoListRepository
{
    Task<PagedModel<TodoListEntity>> GetPagedListAsync(string userName, int page, int pageSize);

    Task<TodoListEntity> GetByIdAsync(int id, string userName);

    Task<TodoListEntity> CreateAsync(TodoListEntity todoListEntity, string userName);

    Task UpdateAsync(int id, TodoListEntity todoListEntity, string userName);

    Task DeleteAsync(int id, string userName);
}
