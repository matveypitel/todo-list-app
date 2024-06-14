using Microsoft.EntityFrameworkCore;
using TodoListApp.WebApi.Data.Entities;

namespace TodoListApp.WebApi.Data;

public class TodoListDbContext : DbContext
{
    public TodoListDbContext(DbContextOptions<TodoListDbContext> options)
        : base(options)
    {
    }

    public DbSet<TodoListEntity> TodoLists { get; set; } = null!;

    public DbSet<TaskItemEntity> Tasks { get; set; } = null!;

    public DbSet<TagEntity> Tags { get; set; } = null!;
}
