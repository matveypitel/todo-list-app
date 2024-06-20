using AutoMapper;
using TodoListApp.Models.Domains;
using TodoListApp.Models.DTOs;
using TodoListApp.WebApi.Data.Entities;
using TodoListApp.WebApi.Interfaces;

namespace TodoListApp.WebApi.Services;

public class CommentDatabaseService : ICommentDatabaseService
{
    private readonly ICommentRepository repository;
    private readonly IMapper mapper;

    public CommentDatabaseService(ICommentRepository repository, IMapper mapper)
    {
        this.repository = repository;
        this.mapper = mapper;
    }

    public async Task<Comment> AddCommentToTaskAsync(Comment comment, string userName)
    {
        var commentEntity = await this.repository.AddToTaskAsync(this.mapper.Map<CommentEntity>(comment), userName);
        return this.mapper.Map<Comment>(commentEntity);
    }

    public async Task DeleteCommentAsync(int id, int taskId, string userName)
    {
        await this.repository.DeleteAsync(id, taskId, userName);
    }

    public async Task<Comment> GetCommentByIdAsync(int id, int taskId)
    {
        var commentEntity = await this.repository.GetByIdAsync(id, taskId);
        return this.mapper.Map<Comment>(commentEntity);
    }

    public async Task<PagedModel<Comment>> GetPagedListOfCommentsAsync(int taskId, int page, int pageSize)
    {
        var comments = await this.repository.GetPagedListOfAllAsync(taskId, page, pageSize);
        return this.mapper.Map<PagedModel<Comment>>(comments);
    }

    public async Task UpdateCommentAsync(int id, int taskId, Comment comment, string userName)
    {
        await this.repository.UpdateAsync(id, taskId, this.mapper.Map<CommentEntity>(comment), userName);
    }
}
