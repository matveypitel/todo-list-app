using AutoMapper;
using TodoListApp.Models.Domains;
using TodoListApp.Models.DTOs;
using TodoListApp.WebApi.Data.Entities;

namespace TodoListApp.WebApi.MappingProfiles;

public class PaginatedApiMappingProfile : Profile
{
    public PaginatedApiMappingProfile()
    {
        _ = this.CreateMap<PagedModel<TodoListEntity>, PagedModel<TodoList>>().ReverseMap();
        _ = this.CreateMap<PagedModel<TodoListModel>, PagedModel<TodoList>>().ReverseMap();

        _ = this.CreateMap<PagedModel<TaskItemEntity>, PagedModel<TaskItem>>().ReverseMap();
        _ = this.CreateMap<PagedModel<TaskItemModel>, PagedModel<TaskItem>>().ReverseMap();
    }
}
