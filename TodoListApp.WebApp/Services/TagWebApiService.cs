using AutoMapper;
using TodoListApp.Models.Domains;
using TodoListApp.Models.DTOs;
using TodoListApp.WebApp.Interfaces;
using TodoListApp.WebApp.Models;
using TodoListApp.WebApp.Utilities;

namespace TodoListApp.WebApp.Services;

public class TagWebApiService : ITagWebApiService
{
    private readonly HttpClient httpClient;
    private readonly IMapper mapper;

    public TagWebApiService(HttpClient httpClient, IMapper mapper)
    {
        this.httpClient = httpClient;
        this.mapper = mapper;
    }

    public async Task<PagedModel<Tag>> GetAllTagsAsync(string token, int page, int pageSize)
    {
        TokenUtility.AddAuthorizationHeader(this.httpClient, token);

        var response = await this.httpClient.GetAsync(new Uri($"api/tags?page={page}&pageSize={pageSize}", UriKind.Relative));
        _ = response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadFromJsonAsync<PagedModel<TagWebApiModel>>();

        return this.mapper.Map<PagedModel<Tag>>(content);
    }

    public async Task<PagedModel<TaskItem>> GetTasksWithTag(string token, string tag, int page, int pageSize)
    {
        TokenUtility.AddAuthorizationHeader(this.httpClient, token);

        var response = await this.httpClient.GetAsync(new Uri($"api/tags/tasks?tag={tag}&page={page}&pageSize={pageSize}", UriKind.Relative));
        _ = response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadFromJsonAsync<PagedModel<TaskItemWebApiModel>>();

        return this.mapper.Map<PagedModel<TaskItem>>(content);
    }

    public async Task<Tag> GetTagByIdAsync(string token, int todoListId, int taskId, int tagId)
    {
        TokenUtility.AddAuthorizationHeader(this.httpClient, token);

        var response = await this.httpClient.GetAsync(new Uri($"api/todolists/{todoListId}/tasks/{taskId}/tags/{tagId}", UriKind.Relative));
        _ = response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadFromJsonAsync<Tag>();

        return this.mapper.Map<Tag>(content);
    }

    public async Task<Tag> AddTagToTaskAsync(string token, int todoListId, int taskId, Tag tag)
    {
        TokenUtility.AddAuthorizationHeader(this.httpClient, token);

        var tagWebApiModel = this.mapper.Map<TagWebApiModel>(tag);

        var response = await this.httpClient.PostAsJsonAsync(new Uri($"api/todolists/{todoListId}/tasks/{taskId}/tags", UriKind.Relative), tagWebApiModel);
        _ = response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadFromJsonAsync<Tag>();

        return this.mapper.Map<Tag>(content);
    }

    public async Task DeleteTagAsync(string token, int todoListId, int taskId, int tagId)
    {
        TokenUtility.AddAuthorizationHeader(this.httpClient, token);

        var response = await this.httpClient.DeleteAsync(new Uri($"api/todolists/{todoListId}/tasks/{taskId}/tags/{tagId}", UriKind.Relative));

        _ = response.EnsureSuccessStatusCode();
    }
}
