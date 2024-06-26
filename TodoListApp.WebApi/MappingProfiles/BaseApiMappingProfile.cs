using AutoMapper;
using TodoListApp.Models.Domains;
using TodoListApp.Models.DTOs;
using TodoListApp.WebApi.Data.Entities;

namespace TodoListApp.WebApi.MappingProfiles;

/// <summary>
/// Represents the base mapping profile for the API.
/// </summary>
public class BaseApiMappingProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BaseApiMappingProfile"/> class.
    /// </summary>
    public BaseApiMappingProfile()
    {
        _ = this.CreateMap<TodoListEntity, TodoList>().ReverseMap()
            .ForMember(dest => dest.Users, opt => opt.MapFrom(src => src.Users));
        _ = this.CreateMap<TodoListModel, TodoList>().ReverseMap()
            .ForMember(dest => dest.Users, opt => opt.MapFrom(src => src.Users));

        _ = this.CreateMap<TodoListUserEntity, TodoListUser>().ReverseMap();
        _ = this.CreateMap<TodoListUserModel, TodoListUser>().ReverseMap();

        _ = this.CreateMap<TaskItem, TaskItemEntity>().ReverseMap()
            .ForMember(dest => dest.Comments, opt => opt.MapFrom(src => src.Comments))
            .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.Tags.Select(
                tag => new Tag
                {
                    Id = tag.Id,
                    Label = tag.Label,
                })));
        _ = this.CreateMap<TaskItem, TaskItemModel>().ReverseMap()
            .ForMember(dest => dest.Comments, opt => opt.MapFrom(src => src.Comments))
            .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.Tags.Select(
                tag => new Tag
                {
                    Id = tag.Id,
                    Label = tag.Label,
                })));

        _ = this.CreateMap<Tag, TagEntity>().ReverseMap()
            .ForMember(dest => dest.Tasks, opt => opt.MapFrom(src => src.Tasks.Select(
                task => new TaskItem
                {
                    Id = task.Id,
                    TodoListId = task.TodoListId,
                    Title = task.Title,
                    Description = task.Description,
                    Owner = task.Owner,
                    AssignedTo = task.AssignedTo,
                })));
        _ = this.CreateMap<Tag, TagModel>().ReverseMap()
            .ForMember(dest => dest.Tasks, opt => opt.MapFrom(src => src.Tasks.Select(
                task => new TaskItem
                {
                    Id = task.Id,
                    TodoListId = task.TodoListId,
                    Title = task.Title,
                    Description = task.Description,
                    Owner = task.Owner ?? string.Empty,
                    AssignedTo = task.AssignedTo,
                })));

        _ = this.CreateMap<Comment, CommentEntity>().ReverseMap();
        _ = this.CreateMap<Comment, CommentModel>().ReverseMap();
    }
}
