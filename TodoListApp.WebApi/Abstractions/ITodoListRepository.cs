using TodoListApp.Models.DTOs;
using TodoListApp.WebApi.Data.Entities;

namespace TodoListApp.WebApi.Abstractions;

public interface ITodoListRepository
{
    Task<PagedModel<TodoListEntity>> GetPagedListAsync(string ownerName, int page, int pageSize);

    Task<TodoListEntity> GetByIdAsync(int id, string ownerName);

    Task<TodoListEntity> CreateAsync(TodoListEntity todoListEntity);

    Task UpdateAsync(int id, TodoListEntity todoListEntity);

    Task DeleteAsync(int id, string ownerName);
}
