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

    public DbSet<CommentEntity> Comments { get; set; } = null!;

    public DbSet<TodoListUserEntity> TodoListsUsers { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ArgumentNullException.ThrowIfNull(modelBuilder);

        base.OnModelCreating(modelBuilder);

        _ = modelBuilder.Entity<TodoListUserEntity>()
            .HasKey(tlu => new { tlu.TodoListId, tlu.UserName });
    }
}
