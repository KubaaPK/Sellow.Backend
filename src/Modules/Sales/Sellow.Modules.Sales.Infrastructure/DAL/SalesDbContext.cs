using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Sellow.Modules.Sales.Core.Categories;

namespace Sellow.Modules.Sales.Infrastructure.DAL;

internal sealed class SalesDbContext : DbContext
{
    public DbSet<Category> Categories { get; set; } = null!;

    public SalesDbContext(DbContextOptions<SalesDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("Sales");
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}