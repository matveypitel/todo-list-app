using TodoListApp.Models.Domains;
using TodoListApp.Models.DTOs;

namespace TodoListApp.WebApi.Interfaces;

public interface ITagDatabaseService
{
    Task<Tag> AddTagToTaskAsync(int taskId, Tag tag);

    Task<PagedModel<Tag>> GetPagedListOfAllAsync(string tasksOwnerName, int page, int pageSize);

    Task<Tag> GetTagByIdAsync(int id, int taskId);

    Task UpdateTagAsync(int id, int taskId, Tag tag);

    Task DeleteTagAsync(int id, int taskId);
}
