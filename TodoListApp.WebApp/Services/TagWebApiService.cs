using AutoMapper;
using TodoListApp.Models.Domains;
using TodoListApp.Models.DTOs;
using TodoListApp.WebApp.Interfaces;
using TodoListApp.WebApp.Models;
using TodoListApp.WebApp.Utilities;

namespace TodoListApp.WebApp.Services;

/// <summary>
/// Represents a service for interacting with the Tag Web API.
/// </summary>
public class TagWebApiService : ITagWebApiService
{
    private readonly HttpClient httpClient;
    private readonly IMapper mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="TagWebApiService"/> class.
    /// </summary>
    /// <param name="httpClient">The HTTP client.</param>
    /// <param name="mapper">The mapper.</param>
    public TagWebApiService(HttpClient httpClient, IMapper mapper)
    {
        this.httpClient = httpClient;
        this.mapper = mapper;
    }

    /// <inheritdoc/>
    public async Task<PagedModel<Tag>> GetAllTagsAsync(string token, int page, int pageSize)
    {
        TokenUtility.AddAuthorizationHeader(this.httpClient, token);

        var response = await this.httpClient.GetAsync(new Uri($"api/tags?page={page}&pageSize={pageSize}", UriKind.Relative));
        _ = response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadFromJsonAsync<PagedModel<TagWebApiModel>>();

        return this.mapper.Map<PagedModel<Tag>>(content);
    }

    /// <inheritdoc/>
    public async Task<PagedModel<TaskItem>> GetTasksWithTag(string token, string tag, int page, int pageSize)
    {
        TokenUtility.AddAuthorizationHeader(this.httpClient, token);

        var response = await this.httpClient.GetAsync(new Uri($"api/tags/tasks?tag={tag}&page={page}&pageSize={pageSize}", UriKind.Relative));
        _ = response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadFromJsonAsync<PagedModel<TaskItemWebApiModel>>();

        return this.mapper.Map<PagedModel<TaskItem>>(content);
    }

    /// <inheritdoc/>
    public async Task<Tag> GetTagByIdAsync(string token, int todoListId, int taskId, int tagId)
    {
        TokenUtility.AddAuthorizationHeader(this.httpClient, token);

        var response = await this.httpClient.GetAsync(new Uri($"api/todolists/{todoListId}/tasks/{taskId}/tags/{tagId}", UriKind.Relative));
        _ = response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadFromJsonAsync<Tag>();

        return this.mapper.Map<Tag>(content);
    }

    /// <inheritdoc/>
    public async Task<Tag> AddTagToTaskAsync(string token, int todoListId, int taskId, Tag tag)
    {
        TokenUtility.AddAuthorizationHeader(this.httpClient, token);

        var tagWebApiModel = this.mapper.Map<TagWebApiModel>(tag);

        var response = await this.httpClient.PostAsJsonAsync(new Uri($"api/todolists/{todoListId}/tasks/{taskId}/tags", UriKind.Relative), tagWebApiModel);
        _ = response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadFromJsonAsync<Tag>();

        return this.mapper.Map<Tag>(content);
    }

    /// <inheritdoc/>
    public async Task DeleteTagAsync(string token, int todoListId, int taskId, int tagId)
    {
        TokenUtility.AddAuthorizationHeader(this.httpClient, token);

        var response = await this.httpClient.DeleteAsync(new Uri($"api/todolists/{todoListId}/tasks/{taskId}/tags/{tagId}", UriKind.Relative));

        _ = response.EnsureSuccessStatusCode();
    }
}
