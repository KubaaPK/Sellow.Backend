using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Sellow.Shared.Infrastructure.Api;
using Sellow.Shared.Infrastructure.Exceptions;

namespace Sellow.Shared.Infrastructure;

internal static class Extensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services
            .AddEndpointsApiExplorer()
            .AddVersioning()
            .AddErrorHandling()
            .AddSwashbuckle()
            .AddControllers()
            .ConfigureApplicationPartManager(manager =>
                manager.FeatureProviders.Add(new InternalControllerFeatureProvider()));

        return services;
    }

    public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder app)
        => app
            .UseHttpsRedirection()
            .UseSwashbuckle()
            .UseErrorHandling();
}