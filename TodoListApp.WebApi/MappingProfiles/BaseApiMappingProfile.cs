using AutoMapper;
using TodoListApp.Models.Domains;
using TodoListApp.Models.DTOs;
using TodoListApp.WebApi.Data.Entities;

namespace TodoListApp.WebApi.MappingProfiles;

public class BaseApiMappingProfile : Profile
{
    public BaseApiMappingProfile()
    {
        _ = this.CreateMap<TodoListEntity, TodoList>().ReverseMap();
        _ = this.CreateMap<TodoListModel, TodoList>().ReverseMap();

        _ = this.CreateMap<TaskItemEntity, TaskItem>().ReverseMap();
        _ = this.CreateMap<TaskItemModel, TaskItem>().ReverseMap();
    }
}
