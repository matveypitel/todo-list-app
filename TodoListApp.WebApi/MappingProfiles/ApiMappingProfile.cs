using AutoMapper;
using TodoListApp.Models.Domains;
using TodoListApp.Models.DTOs;
using TodoListApp.WebApi.Data.Entities;

namespace TodoListApp.WebApi.MappingProfiles;

public class ApiMappingProfile : Profile
{
    public ApiMappingProfile()
    {
        _ = this.CreateMap<TodoListEntity, TodoList>().ReverseMap();
        _ = this.CreateMap<TodoListModel, TodoList>().ReverseMap();
    }
}
