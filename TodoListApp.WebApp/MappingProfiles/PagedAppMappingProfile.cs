using AutoMapper;
using TodoListApp.Models.Domains;
using TodoListApp.Models.DTOs;
using TodoListApp.WebApp.Models;

namespace TodoListApp.WebApp.MappingProfiles;

public class PagedAppMappingProfile : Profile
{
    public PagedAppMappingProfile()
    {
        _ = this.CreateMap<PagedModel<TodoList>, PagedModel<TodoListWebApiModel>>().ReverseMap();
        _ = this.CreateMap<PagedModel<TodoList>, PagedModel<TodoListModel>>().ReverseMap();

        _ = this.CreateMap<PagedModel<TodoListUser>, PagedModel<TodoListUserModel>>().ReverseMap();
        _ = this.CreateMap<PagedModel<TodoListUser>, PagedModel<TodoListUserWebApiModel>>().ReverseMap();

        _ = this.CreateMap<PagedModel<TaskItem>, PagedModel<TaskItemWebApiModel>>().ReverseMap();
        _ = this.CreateMap<PagedModel<TaskItem>, PagedModel<TaskItemModel>>().ReverseMap();

        _ = this.CreateMap<PagedModel<Tag>, PagedModel<TagWebApiModel>>().ReverseMap();
        _ = this.CreateMap<PagedModel<Tag>, PagedModel<TagModel>>().ReverseMap();

        _ = this.CreateMap<PagedModel<Comment>, PagedModel<CommentModel>>().ReverseMap();
        _ = this.CreateMap<PagedModel<Comment>, PagedModel<CommentWebApiModel>>().ReverseMap();
    }
}
