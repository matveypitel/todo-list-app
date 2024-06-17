using AutoMapper;
using TodoListApp.Models.Domains;
using TodoListApp.Models.DTOs;
using TodoListApp.WebApi.Data.Entities;

namespace TodoListApp.WebApi.MappingProfiles;

public class BaseApiMappingProfile : Profile
{
    public BaseApiMappingProfile()
    {
        // TodoList mapping
        _ = this.CreateMap<TodoListEntity, TodoList>().ReverseMap()
            .ForMember(dest => dest.Users, opt => opt.MapFrom(src => src.Users));
        _ = this.CreateMap<TodoListModel, TodoList>().ReverseMap()
            .ForMember(dest => dest.Users, opt => opt.MapFrom(src => src.Users));

        // TodoListUsers mapping
        _ = this.CreateMap<TodoListUserEntity, TodoListUser>().ReverseMap();
        _ = this.CreateMap<TodoListUserModel, TodoListUser>().ReverseMap();

        // Tasks mapping
        _ = this.CreateMap<TaskItem, TaskItemEntity>().ReverseMap()
            .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.Tags.Select(
                tag => new Tag
                {
                    Id = tag.Id,
                    Label = tag.Label,
                })));
        _ = this.CreateMap<TaskItem, TaskItemModel>().ReverseMap()
            .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.Tags.Select(
                tag => new Tag
                {
                    Id = tag.Id,
                    Label = tag.Label,
                })));

        // Tags mapping
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
                    Owner = task.Owner,
                    AssignedTo = task.AssignedTo,
                })));
    }
}
