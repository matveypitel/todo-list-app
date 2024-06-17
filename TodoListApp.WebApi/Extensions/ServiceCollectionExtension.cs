using TodoListApp.WebApi.Interfaces;
using TodoListApp.WebApi.Repositories;
using TodoListApp.WebApi.Services;

namespace TodoListApp.WebApi.Extensions;

public static class ServiceCollectionExtension
{
    public static void AddRepositories(this IServiceCollection services)
    {
        _ = services.AddScoped<ITodoListRepository, TodoListRepository>();
        _ = services.AddScoped<IShareUserRepository, ShareUserRepository>();
        _ = services.AddScoped<ITaskRepository, TaskRepository>();
        _ = services.AddScoped<ITagRepository, TagRepository>();
    }

    public static void AddDatabaseServices(this IServiceCollection services)
    {
        _ = services.AddScoped<ITodoListDatabaseService, TodoListDatabaseService>();
        _ = services.AddScoped<IShareUserDatabaseService, ShareUserDatabaseService>();
        _ = services.AddScoped<ITaskDatabaseService, TaskDatabaseService>();
        _ = services.AddScoped<ITagDatabaseService, TagDatabaseService>();
    }
}
