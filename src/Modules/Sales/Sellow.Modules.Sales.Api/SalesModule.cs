using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Sellow.Modules.Sales.Application;
using Sellow.Modules.Sales.Infrastructure;

namespace Sellow.Modules.Sales.Api;

internal static class SalesModule
{
    public static IServiceCollection AddSalesModule(this IServiceCollection services)
        => services
            .AddApplication()
            .AddInfrastructure();

    public static IApplicationBuilder UseSalesModule(this IApplicationBuilder app)
        => app;
}