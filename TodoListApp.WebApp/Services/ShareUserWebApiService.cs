using System.Threading.Tasks;
using AutoMapper;
using TodoListApp.Models.Domains;
using TodoListApp.Models.DTOs;
using TodoListApp.Models.Enums;
using TodoListApp.WebApp.Interfaces;
using TodoListApp.WebApp.Models;
using TodoListApp.WebApp.Utilities;

namespace TodoListApp.WebApp.Services;

public class ShareUserWebApiService : IShareUserWebApiService
{
    private readonly HttpClient httpClient;
    private readonly IMapper mapper;

    public ShareUserWebApiService(HttpClient httpClient, IMapper mapper)
    {
        this.httpClient = httpClient;
        this.mapper = mapper;
    }

    public async Task<TodoListUser> AddUserToTodoListAsync(string token, int todoListId, TodoListUser user)
    {
        TokenUtility.AddAuthorizationHeader(this.httpClient, token);

        var userWebApiModel = this.mapper.Map<TodoListUserWebApiModel>(user);
        var response = await this.httpClient.PostAsJsonAsync(new Uri($"api/todolists/{todoListId}/share_users", UriKind.Relative), userWebApiModel);
        _ = response.EnsureSuccessStatusCode();

        var createdUserWebApiModel = await response.Content.ReadFromJsonAsync<TodoListUserWebApiModel>();

        return this.mapper.Map<TodoListUser>(createdUserWebApiModel);
    }

    public async Task<PagedModel<TodoListUser>> GetPagedListOfUsersInTodoListAsync(string token, int todoListId, int page, int pageSize)
    {
        TokenUtility.AddAuthorizationHeader(this.httpClient, token);

        var response = await this.httpClient.GetAsync(new Uri($"api/todolists/{todoListId}/share_users?page={page}&pageSize={pageSize}", UriKind.Relative));
        _ = response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadFromJsonAsync<PagedModel<TodoListUserWebApiModel>>();

        return this.mapper.Map<PagedModel<TodoListUser>>(content);
    }

    public async Task<TodoListUser> GetUserInTodoListAsync(string token, int todoListId, string userName)
    {
        TokenUtility.AddAuthorizationHeader(this.httpClient, token);

        var response = await this.httpClient.GetAsync(new Uri($"api/todolists/{todoListId}/share_users/{userName}", UriKind.Relative));

        _ = response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadFromJsonAsync<TodoListUserWebApiModel>();

        return this.mapper.Map<TodoListUser>(content);
    }

    public async Task RemoveUserFromTodoListAsync(string token, int todoListId, string userName)
    {
        TokenUtility.AddAuthorizationHeader(this.httpClient, token);

        var response = await this.httpClient.DeleteAsync(new Uri($"api/todolists/{todoListId}/share_users/{userName}", UriKind.Relative));

        _ = response.EnsureSuccessStatusCode();
    }

    public async Task UpdateUserRoleAsync(string token, int todoListId, string userName, TodoListUser user)
    {
        TokenUtility.AddAuthorizationHeader(this.httpClient, token);

        var todoListUserWebApiModel = this.mapper.Map<TodoListUserWebApiModel>(user);
        var response = await this.httpClient.PutAsJsonAsync(new Uri($"api/todolists/{todoListId}/share_users/{userName}", UriKind.Relative), todoListUserWebApiModel);

        _ = response.EnsureSuccessStatusCode();
    }
}
