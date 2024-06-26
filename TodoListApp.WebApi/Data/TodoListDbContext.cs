using Microsoft.EntityFrameworkCore;
using TodoListApp.WebApi.Data.Entities;

namespace TodoListApp.WebApi.Data;

/// <summary>
/// Represents the database context for the TodoList application.
/// </summary>
public class TodoListDbContext : DbContext
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TodoListDbContext"/> class.
    /// </summary>
    /// <param name="options">The options for configuring the context.</param>
    public TodoListDbContext(DbContextOptions<TodoListDbContext> options)
        : base(options)
    {
    }

    /// <summary>
    /// Gets or sets the DbSet of TodoListEntity.
    /// </summary>
    public DbSet<TodoListEntity> TodoLists { get; set; } = null!;

    /// <summary>
    /// Gets or sets the DbSet of TaskItemEntity.
    /// </summary>
    public DbSet<TaskItemEntity> Tasks { get; set; } = null!;

    /// <summary>
    /// Gets or sets the DbSet of TagEntity.
    /// </summary>
    public DbSet<TagEntity> Tags { get; set; } = null!;

    /// <summary>
    /// Gets or sets the DbSet of CommentEntity.
    /// </summary>
    public DbSet<CommentEntity> Comments { get; set; } = null!;

    /// <summary>
    /// Gets or sets the DbSet of TodoListUserEntity.
    /// </summary>
    public DbSet<TodoListUserEntity> TodoListsUsers { get; set; } = null!;

    /// <summary>
    /// Configures the model for the database context.
    /// </summary>
    /// <param name="modelBuilder">The model builder.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ArgumentNullException.ThrowIfNull(modelBuilder);

        base.OnModelCreating(modelBuilder);

        _ = modelBuilder.Entity<TodoListUserEntity>()
            .HasKey(tlu => new { tlu.TodoListId, tlu.UserName });
    }
}
