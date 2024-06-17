using AutoMapper;
using TodoListApp.Models.Domains;
using TodoListApp.Models.DTOs;
using TodoListApp.WebApi.Data.Entities;

namespace TodoListApp.WebApi.MappingProfiles;

public class PagedApiMappingProfile : Profile
{
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
    }
}
