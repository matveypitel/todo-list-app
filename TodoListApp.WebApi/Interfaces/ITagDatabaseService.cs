using TodoListApp.Models.Domains;
using TodoListApp.Models.DTOs;

namespace TodoListApp.WebApi.Interfaces;

public interface ITagDatabaseService
{
    Task<Tag> AddTagToTaskAsync(int taskId, Tag tag, string userName);

    Task<PagedModel<Tag>> GetPagedListOfAllAsync(string userName, int page, int pageSize);

    Task<Tag> GetTagByIdAsync(int id, int taskId);

    Task DeleteTagAsync(int id, int taskId, string userName);
}
