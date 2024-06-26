using AutoMapper;
using TodoListApp.Models.Domains;
using TodoListApp.Models.DTOs;
using TodoListApp.WebApi.Data.Entities;

namespace TodoListApp.WebApi.MappingProfiles;

/// <summary>
/// Represents a mapping profile for Paged API models.
/// </summary>
public class PagedApiMappingProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PagedApiMappingProfile"/> class.
    /// </summary>
    public PagedApiMappingProfile()
    {
        _ = this.CreateMap<PagedModel<TodoListEntity>, PagedModel<TodoList>>().ReverseMap();
        _ = this.CreateMap<PagedModel<TodoListModel>, PagedModel<TodoList>>().ReverseMap();

        _ = this.CreateMap<PagedModel<TodoListUserModel>, PagedModel<TodoListUser>>().ReverseMap();
        _ = this.CreateMap<PagedModel<TodoListUserEntity>, PagedModel<TodoListUser>>().ReverseMap();

        _ = this.CreateMap<PagedModel<TaskItemEntity>, PagedModel<TaskItem>>().ReverseMap();
        _ = this.CreateMap<PagedModel<TaskItemModel>, PagedModel<TaskItem>>().ReverseMap();

        _ = this.CreateMap<PagedModel<TagEntity>, PagedModel<Tag>>().ReverseMap();
        _ = this.CreateMap<PagedModel<TagModel>, PagedModel<Tag>>().ReverseMap();

        _ = this.CreateMap<PagedModel<CommentEntity>, PagedModel<Comment>>().ReverseMap();
        _ = this.CreateMap<PagedModel<CommentModel>, PagedModel<Comment>>().ReverseMap();
    }
}
