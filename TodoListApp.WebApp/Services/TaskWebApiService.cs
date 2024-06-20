using AutoMapper;
using TodoListApp.Models.Domains;
using TodoListApp.Models.DTOs;
using TodoListApp.Models.Enums;
using TodoListApp.WebApp.Interfaces;
using TodoListApp.WebApp.Models;
using TodoListApp.WebApp.Utilities;

namespace TodoListApp.WebApp.Services;

public class TaskWebApiService : ITaskWebApiService
{
    private readonly HttpClient httpClient;
    private readonly IMapper mapper;

    public TaskWebApiService(HttpClient httpClient, IMapper mapper)
    {
        this.httpClient = httpClient;
        this.mapper = mapper;
    }

    public async Task<TodoListRole> GetUserRoleInTodoListAsync(string token, int todoListId)
    {
        TokenUtility.AddAuthorizationHeader(this.httpClient, token);

        var response = await this.httpClient.GetAsync(new Uri($"api/todolists/{todoListId}/tasks/role", UriKind.Relative));

        _ = response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<TodoListRole>();
    }

    public async Task<TaskItem> CreateTaskAsync(string token, int todoListId, TaskItem task)
    {
        TokenUtility.AddAuthorizationHeader(this.httpClient, token);

        var taskWebApiModel = this.mapper.Map<TaskItemWebApiModel>(task);
        var response = await this.httpClient.PostAsJsonAsync(new Uri($"api/todolists/{todoListId}/tasks", UriKind.Relative), taskWebApiModel);
        _ = response.EnsureSuccessStatusCode();

        var createdTaskWebApiModel = await response.Content.ReadFromJsonAsync<TaskItemWebApiModel>();

        return this.mapper.Map<TaskItem>(createdTaskWebApiModel);
    }

    public async Task DeleteTaskAsync(string token, int id, int todoListId)
    {
        TokenUtility.AddAuthorizationHeader(this.httpClient, token);

        var response = await this.httpClient.DeleteAsync(new Uri($"api/todolists/{todoListId}/tasks/{id}", UriKind.Relative));

        _ = response.EnsureSuccessStatusCode();
    }

    public async Task<TaskItem> GetAssignedTaskByIdAsync(string token, int id)
    {
        TokenUtility.AddAuthorizationHeader(this.httpClient, token);

        var response = await this.httpClient.GetAsync(new Uri($"api/tasks/assigned_to_me/{id}", UriKind.Relative));

        _ = response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadFromJsonAsync<TaskItemWebApiModel>();

        return this.mapper.Map<TaskItem>(content);
    }

    public async Task<PagedModel<TaskItem>> GetAssignedTasksToUserAsync(string token, int page, int pageSize, string? status, string? sort)
    {
        TokenUtility.AddAuthorizationHeader(this.httpClient, token);

        var response = await this.httpClient.GetAsync(new Uri($"api/tasks/assigned_to_me?page={page}&pageSize={pageSize}&status={status}&sort={sort}", UriKind.Relative));
        _ = response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadFromJsonAsync<PagedModel<TaskItemWebApiModel>>();

        return this.mapper.Map<PagedModel<TaskItem>>(content);
    }

    public async Task<PagedModel<TaskItem>> GetPagedTasksAsync(string token, int todoListId, int page, int pageSize)
    {
        TokenUtility.AddAuthorizationHeader(this.httpClient, token);

        var response = await this.httpClient.GetAsync(new Uri($"api/todolists/{todoListId}/tasks?page={page}&pageSize={pageSize}", UriKind.Relative));
        _ = response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadFromJsonAsync<PagedModel<TaskItemWebApiModel>>();

        return this.mapper.Map<PagedModel<TaskItem>>(content);
    }

    public async Task<PagedModel<TaskItem>> GetPagedSearchedTaskAsync(string token, int page, int pageSize, string? title, string? creationDate, string? dueDate)
    {
        TokenUtility.AddAuthorizationHeader(this.httpClient, token);

        var response = await this.httpClient.GetAsync(new Uri($"api/search/tasks?page={page}&pageSize={pageSize}&title={title}&creationDate={creationDate}&dueDate={dueDate}", UriKind.Relative));
        _ = response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadFromJsonAsync<PagedModel<TaskItemWebApiModel>>();

        return this.mapper.Map<PagedModel<TaskItem>>(content);
    }

    public async Task<TaskItem> GetTaskByIdAsync(string token, int id, int todoListId)
    {
        TokenUtility.AddAuthorizationHeader(this.httpClient, token);

        var response = await this.httpClient.GetAsync(new Uri($"api/todolists/{todoListId}/tasks/{id}", UriKind.Relative));
        _ = response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadFromJsonAsync<TaskItemWebApiModel>();

        return this.mapper.Map<TaskItem>(content);
    }

    public async Task UpdateTaskAsync(string token, int id, int todoListId, TaskItem task)
    {
        TokenUtility.AddAuthorizationHeader(this.httpClient, token);

        var taskWebApiModel = this.mapper.Map<TaskItemWebApiModel>(task);
        var response = await this.httpClient.PutAsJsonAsync(new Uri($"api/todolists/{todoListId}/tasks/{id}", UriKind.Relative), taskWebApiModel);

        _ = response.EnsureSuccessStatusCode();
    }

    public async Task UpdateTaskStatusAsync(string token, int id, TaskItem task)
    {
        TokenUtility.AddAuthorizationHeader(this.httpClient, token);

        var taskWebApiModel = this.mapper.Map<TaskItemWebApiModel>(task);

        var response = await this.httpClient.PutAsJsonAsync(new Uri($"api/tasks/assigned_to_me/status/{id}", UriKind.Relative), taskWebApiModel);
        _ = response.EnsureSuccessStatusCode();
    }
}
