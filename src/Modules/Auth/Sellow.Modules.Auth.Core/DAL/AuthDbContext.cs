using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Sellow.Modules.Auth.Core.Domain;

namespace Sellow.Modules.Auth.Core.DAL;

internal sealed class AuthDbContext : DbContext
{
    public DbSet<User> Users { get; set; }

    public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("Auth");
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}