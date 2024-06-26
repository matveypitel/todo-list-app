using AutoMapper;
using TodoListApp.Models.Domains;
using TodoListApp.Models.DTOs;
using TodoListApp.WebApp.Interfaces;
using TodoListApp.WebApp.Models;
using TodoListApp.WebApp.Utilities;

namespace TodoListApp.WebApp.Services;

/// <summary>
/// Represents a service for interacting with the TodoList Web API.
/// </summary>
public class TodoListWebApiService : ITodoListWebApiService
{
    private readonly HttpClient httpClient;
    private readonly IMapper mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="TodoListWebApiService"/> class.
    /// </summary>
    /// <param name="httpClient">The HTTP client.</param>
    /// <param name="mapper">The mapper.</param>
    public TodoListWebApiService(HttpClient httpClient, IMapper mapper)
    {
        this.httpClient = httpClient;
        this.mapper = mapper;
    }

    /// <inheritdoc/>
    public async Task<PagedModel<TodoList>> GetPagedListOfTodoListsAsync(string token, int page, int pageSize)
    {
        TokenUtility.AddAuthorizationHeader(this.httpClient, token);

        var response = await this.httpClient.GetAsync(new Uri($"api/todolists?page={page}&pageSize={pageSize}", UriKind.Relative));
        _ = response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadFromJsonAsync<PagedModel<TodoListWebApiModel>>();

        return this.mapper.Map<PagedModel<TodoList>>(content);
    }

    /// <inheritdoc/>
    public async Task<TodoList> GetTodoListByIdAsync(string token, int id)
    {
        TokenUtility.AddAuthorizationHeader(this.httpClient, token);

        var response = await this.httpClient.GetAsync(new Uri($"api/todolists/{id}", UriKind.Relative));
        _ = response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadFromJsonAsync<TodoListWebApiModel>();

        return this.mapper.Map<TodoList>(content);
    }

    /// <inheritdoc/>
    public async Task<TodoList> CreateTodoListAsync(string token, TodoList todoList)
    {
        TokenUtility.AddAuthorizationHeader(this.httpClient, token);

        var todoListWebApiModel = this.mapper.Map<TodoListWebApiModel>(todoList);
        var response = await this.httpClient.PostAsJsonAsync(new Uri("api/todolists", UriKind.Relative), todoListWebApiModel);
        _ = response.EnsureSuccessStatusCode();

        var createdTodoListWebApiModel = await response.Content.ReadFromJsonAsync<TodoListWebApiModel>();

        return this.mapper.Map<TodoList>(createdTodoListWebApiModel);
    }

    /// <inheritdoc/>
    public async Task UpdateTodoListAsync(string token, int id, TodoList todoList)
    {
        TokenUtility.AddAuthorizationHeader(this.httpClient, token);

        var todoListWebApiModel = this.mapper.Map<TodoListWebApiModel>(todoList);
        var response = await this.httpClient.PutAsJsonAsync(new Uri($"api/todolists/{id}", UriKind.Relative), todoListWebApiModel);

        _ = response.EnsureSuccessStatusCode();
    }

    /// <inheritdoc/>
    public async Task DeleteTodoListAsync(string token, int id)
    {
        TokenUtility.AddAuthorizationHeader(this.httpClient, token);

        var response = await this.httpClient.DeleteAsync(new Uri($"api/todolists/{id}", UriKind.Relative));

        _ = response.EnsureSuccessStatusCode();
    }
}
