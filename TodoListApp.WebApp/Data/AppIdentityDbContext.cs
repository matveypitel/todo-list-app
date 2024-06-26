using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace TodoListApp.WebApp.Data;

/// <summary>
/// Represents the application-specific identity database context.
/// </summary>
public class AppIdentityDbContext : IdentityDbContext<IdentityUser>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AppIdentityDbContext"/> class.
    /// </summary>
    /// <param name="options">The options for the database context.</param>
    public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options)
        : base(options)
    {
    }
}
