using AutoMapper;
using TodoListApp.Models.Domains;
using TodoListApp.Models.DTOs;
using TodoListApp.WebApp.Models;

namespace TodoListApp.WebApp.MappingProfiles;

public class BaseAppMappingProfile : Profile
{
    public BaseAppMappingProfile()
    {
        _ = this.CreateMap<TodoList, TodoListWebApiModel>().ReverseMap();
        _ = this.CreateMap<TodoList, TodoListModel>().ReverseMap().ReverseMap();

        _ = this.CreateMap<TodoListUser, TodoListUserWebApiModel>().ReverseMap();
        _ = this.CreateMap<TodoListUser, TodoListUserModel>().ReverseMap();

        _ = this.CreateMap<TaskItem, TaskItemWebApiModel>().ReverseMap()
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

        _ = this.CreateMap<Tag, TagWebApiModel>().ReverseMap()
            .ForMember(dest => dest.Tasks, opt => opt.MapFrom(src => src.Tasks.Select(
                task => new TaskItem
                {
                    Id = task.Id,
                    TodoListId = task.TodoListId,
                    Title = task.Title,
                    Description = task.Description,
                })));
        _ = this.CreateMap<Tag, TagModel>().ReverseMap()
            .ForMember(dest => dest.Tasks, opt => opt.MapFrom(src => src.Tasks.Select(
                task => new TaskItem
                {
                    Id = task.Id,
                    TodoListId = task.TodoListId,
                    Title = task.Title,
                    Description = task.Description,
                })));

        _ = this.CreateMap<Comment, CommentModel>().ReverseMap();
        _ = this.CreateMap<Comment, CommentWebApiModel>().ReverseMap();
    }
}
