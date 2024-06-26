using TodoListApp.WebApi.Interfaces;
using TodoListApp.WebApi.Repositories;
using TodoListApp.WebApi.Services;

namespace TodoListApp.WebApi.Extensions;

/// <summary>
/// Extension methods for IServiceCollection.
/// </summary>
public static class ServiceCollectionExtension
{
    /// <summary>
    /// Adds repositories to the service collection.
    /// </summary>
    /// <param name="services">The IServiceCollection instance.</param>
    public static void AddRepositories(this IServiceCollection services)
    {
        _ = services.AddScoped<ITodoListRepository, TodoListRepository>();
        _ = services.AddScoped<IShareUserRepository, ShareUserRepository>();
        _ = services.AddScoped<ITaskRepository, TaskRepository>();
        _ = services.AddScoped<ITagRepository, TagRepository>();
        _ = services.AddScoped<ICommentRepository, CommentRepository>();
    }

    /// <summary>
    /// Adds database services to the service collection.
    /// </summary>
    /// <param name="services">The IServiceCollection instance.</param>
    public static void AddDatabaseServices(this IServiceCollection services)
    {
        _ = services.AddScoped<ITodoListDatabaseService, TodoListDatabaseService>();
        _ = services.AddScoped<IShareUserDatabaseService, ShareUserDatabaseService>();
        _ = services.AddScoped<ITaskDatabaseService, TaskDatabaseService>();
        _ = services.AddScoped<ITagDatabaseService, TagDatabaseService>();
        _ = services.AddScoped<ICommentDatabaseService, CommentDatabaseService>();
    }
}
