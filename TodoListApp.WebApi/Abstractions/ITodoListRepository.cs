using TodoListApp.WebApi.Data.Entities;

namespace TodoListApp.WebApi.Abstractions;

public interface ITodoListRepository
{
    Task<IEnumerable<TodoListEntity>> GetAllAsync(string userId, int page, int pageSize);

    Task<TodoListEntity> GetByIdAsync(int id, string userId);

    Task<TodoListEntity> CreateAsync(TodoListEntity todoListEntity);

    Task UpdateAsync(int id, TodoListEntity todoListEntity);

    Task DeleteAsync(int id, string userId);

    Task<int> GetCountAsync(string userId);
}
