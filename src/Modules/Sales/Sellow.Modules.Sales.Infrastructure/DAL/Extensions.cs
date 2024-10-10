using Microsoft.Extensions.DependencyInjection;
using Sellow.Modules.Sales.Core.Categories;
using Sellow.Modules.Sales.Infrastructure.DAL.Repositories;
using Sellow.Shared.Infrastructure.DAL.Postgres;

namespace Sellow.Modules.Sales.Infrastructure.DAL;

internal static class Extensions
{
    public static IServiceCollection AddDatabase(this IServiceCollection services)
        => services
            .AddPostgres<SalesDbContext>()
            .AddScoped<ICategoryRepository, CategoryRepository>();
    
}