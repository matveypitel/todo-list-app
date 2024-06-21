using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TodoListApp.Models.Domains;
using TodoListApp.Models.DTOs;
using TodoListApp.WebApp.Interfaces;
using TodoListApp.WebApp.Models;
using TodoListApp.WebApp.Utilities;

namespace TodoListApp.WebApp.Services;

public class CommentWebApiService : ICommentWebApiService
{
    private readonly HttpClient httpClient;
    private readonly IMapper mapper;

    public CommentWebApiService(HttpClient httpClient, IMapper mapper)
    {
        this.httpClient = httpClient;
        this.mapper = mapper;
    }

    public async Task<Comment> AddCommentToTaskAsync(string token, int todoListId, int taskId, Comment comment)
    {
        ArgumentNullException.ThrowIfNull(comment);
        TokenUtility.AddAuthorizationHeader(this.httpClient, token);

        var commentWebApiModel = this.mapper.Map<CommentWebApiModel>(comment);
        var response = await this.httpClient.PostAsJsonAsync(new Uri($"api/todolists/{todoListId}/tasks/{taskId}/comments", UriKind.Relative), commentWebApiModel);
        _ = response.EnsureSuccessStatusCode();

        var createdCommentWebApiModel = await response.Content.ReadFromJsonAsync<CommentWebApiModel>();

        return this.mapper.Map<Comment>(createdCommentWebApiModel);
    }

    public async Task DeleteCommentAsync(string token, int id, int todoListId, int taskId)
    {
        TokenUtility.AddAuthorizationHeader(this.httpClient, token);

        var response = await this.httpClient.DeleteAsync(new Uri($"api/todolists/{todoListId}/tasks/{taskId}/comments/{id}", UriKind.Relative));

        _ = response.EnsureSuccessStatusCode();
    }

    public async Task<Comment> GetCommentByIdAsync(string token, int id, int todoListId, int taskId)
    {
        TokenUtility.AddAuthorizationHeader(this.httpClient, token);

        var response = await this.httpClient.GetAsync(new Uri($"api/todolists/{todoListId}/tasks/{taskId}/comments/{id}", UriKind.Relative));
        _ = response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadFromJsonAsync<CommentWebApiModel>();

        return this.mapper.Map<Comment>(content);
    }

    public async Task<PagedModel<Comment>> GetPagedListOfCommentsAsync(string token, int taskId, int todoListId, int page, int pageSize)
    {
        TokenUtility.AddAuthorizationHeader(this.httpClient, token);

        var response = await this.httpClient.GetAsync(new Uri($"api/todolists/{todoListId}/tasks/{taskId}/comments?page={page}&pageSize={pageSize}", UriKind.Relative));
        _ = response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadFromJsonAsync<PagedModel<CommentWebApiModel>>();

        return this.mapper.Map<PagedModel<Comment>>(content);
    }

    public async Task UpdateCommentAsync(string token, int id, int todoListId, int taskId, Comment comment)
    {
        TokenUtility.AddAuthorizationHeader(this.httpClient, token);

        var commentWebApiModel = this.mapper.Map<CommentWebApiModel>(comment);
        var response = await this.httpClient.PutAsJsonAsync(new Uri($"api/todolists/{todoListId}/tasks/{taskId}/comments/{id}", UriKind.Relative), commentWebApiModel);

        _ = response.EnsureSuccessStatusCode();
    }
}
