using TodoListApp.Models.Domains;
using TodoListApp.Models.DTOs;

namespace TodoListApp.WebApp.Interfaces;

public interface ITagWebApiService
{
    Task<Tag> AddTagToTaskAsync(string token, int todoListId, int taskId, Tag tag);

    Task DeleteTagAsync(string token, int todoListId, int taskId, int tagId);

    Task<PagedModel<Tag>> GetAllTagsAsync(string token, int page, int pageSize);
    Task<Tag> GetTagByIdAsync(string token, int todoListId, int taskId, int tagId);
    Task<PagedModel<TaskItem>> GetTasksWithTag(string token, string tag, int page, int pageSize);
}
