using TodoListApp.WebApi.Interfaces;
using TodoListApp.WebApi.Repositories;
using TodoListApp.WebApi.Services;

namespace TodoListApp.WebApi.Extensions;

public static class ServiceCollectionExtension
{
    public static void AddRepositories(this IServiceCollection services)
    {
        _ = services.AddScoped<ITodoListRepository, TodoListRepository>();
        _ = services.AddScoped<ITaskRepository, TaskRepository>();
    }

    public static void AddDatabaseServices(this IServiceCollection services)
    {
        _ = services.AddScoped<ITodoListDatabaseService, TodoListDatabaseService>();
        _ = services.AddScoped<ITaskDatabaseService, TaskDatabaseService>();
    }
}
