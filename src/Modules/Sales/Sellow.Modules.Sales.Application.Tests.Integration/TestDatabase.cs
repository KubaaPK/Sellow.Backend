using Microsoft.EntityFrameworkCore;
using Sellow.Modules.Sales.Infrastructure.DAL;

namespace Sellow.Modules.Sales.Application.Tests.Integration;

internal sealed class TestDatabase : IDisposable
{
    public SalesDbContext Context { get; }

    public TestDatabase()
    {
        var connectionString =
            $"Server=localhost;Port=5432;Database=Sellow-Sales-Tests-{Guid.NewGuid()};User Id=postgres;Password=postgres;Include Error Detail=true;";
        Context = new SalesDbContext(new DbContextOptionsBuilder<SalesDbContext>().UseNpgsql(connectionString).Options);
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    }

    public Task Init() => Context.Database.EnsureCreatedAsync();

    public void Dispose()
    {
        Context.Database.EnsureDeleted();
        Context.Dispose();
    }
}