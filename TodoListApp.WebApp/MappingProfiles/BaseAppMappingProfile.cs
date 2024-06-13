using AutoMapper;
using TodoListApp.Models.Domains;
using TodoListApp.Models.DTOs;
using TodoListApp.WebApp.Models;

namespace TodoListApp.WebApp.MappingProfiles;

public class BaseAppMappingProfile : Profile
{
    public BaseAppMappingProfile()
    {
        _ = this.CreateMap<TodoListWebApiModel, TodoList>().ReverseMap();
        _ = this.CreateMap<TodoListModel, TodoList>().ReverseMap().ReverseMap();

        _ = this.CreateMap<TaskItem, TaskItemWebApiModel>().ReverseMap();
        _ = this.CreateMap<TaskItem, TaskItemModel>().ReverseMap();
    }
}
