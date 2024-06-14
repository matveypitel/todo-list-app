using AutoMapper;
using TodoListApp.Models.Domains;
using TodoListApp.Models.DTOs;
using TodoListApp.WebApi.Data.Entities;
using TodoListApp.WebApi.Interfaces;

namespace TodoListApp.WebApi.Services;

public class TagDatabaseService : ITagDatabaseService
{
    private readonly ITagRepository repository;
    private readonly IMapper mapper;

    public TagDatabaseService(ITagRepository repository, IMapper mapper)
    {
        this.repository = repository;
        this.mapper = mapper;
    }

    public async Task<Tag> AddTagToTaskAsync(int taskId, Tag tag)
    {
        var tagEntity = await this.repository.AddToTaskAsync(taskId, this.mapper.Map<TagEntity>(tag));
        return this.mapper.Map<Tag>(tagEntity);
    }

    public async Task<PagedModel<Tag>> GetPagedListOfAllAsync(string tasksOwnerName, int page, int pageSize)
    {
        var tags = await this.repository.GetPagedListOfAllAsync(tasksOwnerName, page, pageSize);
        return this.mapper.Map<PagedModel<Tag>>(tags);
    }

    public async Task<Tag> GetTagByIdAsync(int id, int taskId)
    {
        var tag = await this.repository.GetByIdAsync(id, taskId);
        return this.mapper.Map<Tag>(tag);
    }

    public async Task UpdateTagAsync(int id, int taskId, Tag tag)
    {
        var tagEntity = this.mapper.Map<TagEntity>(tag);
        await this.repository.UpdateAsync(id, taskId, tagEntity);
    }

    public async Task DeleteTagAsync(int id, int taskId)
    {
        await this.repository.DeleteAsync(id, taskId);
    }
}
